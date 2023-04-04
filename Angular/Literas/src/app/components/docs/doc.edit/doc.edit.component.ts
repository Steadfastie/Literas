import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {QuillEditorComponent} from "ngx-quill";
import {debounceTime, distinctUntilChanged, Subject, takeUntil} from "rxjs";
import {SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import * as docSelectors from 'src/app/state/selectors/docs.crud.selectors';
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import {Store} from "@ngrx/store";
import {DocResponseModel} from "../../../models/docs/docs.response.model";
import {ActivatedRoute} from "@angular/router";
import {Guid} from "guid-typescript";

@Component({
  selector: 'doc-edit',
  templateUrl: './doc.edit.component.html',
  styleUrls: ['./doc.edit.component.sass']
})
export class DocEditComponent implements OnInit, OnDestroy, AfterViewInit{
  urlGuid?: Guid;
  fetchedDoc?: DocResponseModel;
  value!: any;
  editForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    content: ['', [Validators.required, Validators.minLength(3)]]
  });
  @ViewChild('titleQuill', {static: true}) title!: QuillEditorComponent;
  @ViewChild('contentQuill', {static: true}) content!: QuillEditorComponent;
  linkInputOpenState: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private fb: FormBuilder,
              private store: Store,
              private activatedRoute: ActivatedRoute){}
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      const guid = params['id'];
      if (this.urlGuid !== Guid.parse(guid)){
        this.urlGuid = Guid.parse(guid);
        this.store.dispatch(docCrudActions.url_id_change({id: this.urlGuid.toString()}));
        if (this.fetchedDoc && this.fetchedDoc?.id !== this.urlGuid.toString()){
          this.store.dispatch(docCrudActions.doc_fetch({id: this.urlGuid.toString()}));
        }
      }
    });

    this.store.select(docSelectors.selectCurrentDocLastSave)
      .pipe(takeUntil(this.subManager$))
      .subscribe(doc => {
        if (doc == undefined) {
          this.store.dispatch(docCrudActions.doc_fetch({id: this.urlGuid!.toString()}));
          return;
        }
        let prevId = this.fetchedDoc?.id
        this.fetchedDoc = doc;
        if ((this.editForm.pristine || prevId !== this.fetchedDoc?.id) && this.content.quillEditor){
          this.loadForm();
        }
      });

    this.editForm.valueChanges.pipe(
        takeUntil(this.subManager$),
        debounceTime(1000),
        distinctUntilChanged()
      ).subscribe(value => {
        if (this.fetchedDoc?.title != value.title ||
          this.fetchedDoc?.content != value.content){
          this.submit();
        }
      })
  }
  ngAfterViewInit(): void {
    this.title.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '2.5rem'};
    this.content.styles = {'min-width':'fit-content', 'font-family': 'Sanchez, serif', 'font-size': '1rem'};

    this.content.elementRef.nativeElement.addEventListener('click', (event: Event) => {
      const target = event.target as HTMLElement;

      if (target.tagName === 'A') {
        event.preventDefault();
        window.open(target.getAttribute('href')!, '_blank');
      }
    });

    /*
    this.content.onContentChanged
      .pipe(
        takeUntil(this.subManager$),
        debounceTime(1000),
        distinctUntilChanged()
      )
      .subscribe(value => {
        this.submit();
      });*/
  }
  loadForm(){
    if (!this.content.quillEditor) return;

    this.editForm.patchValue({
      title: this.fetchedDoc?.title,
    });
    let deltas = JSON.parse(this.fetchedDoc!.content);
    this.content.quillEditor.setContents(deltas);
  }
  submit(){
    let contentFromEditor = this.content.quillEditor.getContents();
    if (this.editForm.valid){
      let docFromForm = {
        id: this.urlGuid!.toString(),
        title: this.title?.quillEditor.getText()!,
        content: JSON.stringify(contentFromEditor)
      }

      if (docFromForm.title == this.fetchedDoc?.title && docFromForm.content == this.fetchedDoc.content) return;

      this.store.dispatch(docCrudActions.doc_save());
      this.store.dispatch(docCrudActions.doc_patch(docFromForm));
    }
  }
  adaptToolBar(selectionChange: SelectionChange){
    if (this.linkInputOpenState && selectionChange.oldRange){
      selectionChange.editor.formatText(
        selectionChange.oldRange.index,
        selectionChange.oldRange.length,
        'background', '#338dfa'
      );
      selectionChange.editor.formatText(
        selectionChange.oldRange.index,
        selectionChange.oldRange.length,
        'color', 'white'
      );
      return;
    }

    let range = selectionChange.range!;
    if (range.length !==0 ){
      let selectedText = selectionChange.editor.getText(range.index, range.length);
      let selectedTextFormats = selectionChange.editor.getFormat(range.index, range.length);

      const selection = window.getSelection()!;
      const rangeRect = selection.getRangeAt(0).getBoundingClientRect();

      this.store.dispatch(quillSelectionActions.quill_newSelection({
        range: range,
        bounds: {left: rangeRect.x, top: rangeRect.y},
        text: selectedText,
        formats: selectedTextFormats,
        linkInputOpenState: false
      }));
    }
    else {
      this.store.dispatch(quillSelectionActions.quill_focusOff());
    }
  }

  ngOnDestroy(): void {
    this.submit();
    this.subManager$.next('destroyed');
    this.store.dispatch(docCrudActions.doc_clear_last_save());
    this.store.dispatch(docCrudActions.url_id_change({id: undefined}));
  }
}
