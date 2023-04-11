import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {QuillEditorComponent} from "ngx-quill";
import {debounceTime, distinctUntilChanged, skip, Subject, takeUntil} from "rxjs";
import {SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import * as quillSelectionSelectors from 'src/app/state/selectors/quill.selection.selectors';
import * as docSelectors from 'src/app/state/selectors/docs.crud.selectors';
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import {Store} from "@ngrx/store";
import {DocResponseModel} from "../../../models/docs/docs.response.model";
import {ActivatedRoute} from "@angular/router";
import {Guid} from "guid-typescript";
import {Delta} from "quill";
import {isEqual} from 'lodash'

@Component({
  selector: 'doc-edit',
  templateUrl: './doc.edit.component.html',
  styleUrls: ['./doc.edit.component.sass']
})
export class DocEditComponent implements OnInit, OnDestroy, AfterViewInit{
  urlGuid?: Guid;
  fetchedDoc?: DocResponseModel;
  @ViewChild('titleQuill', {static: true}) title!: QuillEditorComponent;
  @ViewChild('contentQuill', {static: true}) content!: QuillEditorComponent;
  toolbarOpened: boolean = false;
  linkInputOpened: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private fb: FormBuilder,
              private store: Store,
              private activatedRoute: ActivatedRoute){
    this.store.select(quillSelectionSelectors.selectToolbarOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(toolbarOpened => this.toolbarOpened = toolbarOpened);
  }
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
        if (this.content.quillEditor && prevId !== this.fetchedDoc?.id){
          this.loadForm();
        }
      });

    this.content.onContentChanged
      .pipe(
        skip(1),
        takeUntil(this.subManager$),
        debounceTime(1000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.submit();
      });
  }
  ngAfterViewInit(): void {
    this.content.elementRef.nativeElement.addEventListener('click', (event: Event) => {
      const target = event.target as HTMLElement;

      if (target.tagName === 'A') {
        event.preventDefault();
        window.open(target.getAttribute('href')!, '_blank');
      }
    });
  }
  loadForm(){
    if (!this.content.quillEditor || !this.title.quillEditor) return;

    let titleDelta = JSON.parse(this.fetchedDoc!.title) as Delta;
    let contentDeltas = JSON.parse(this.fetchedDoc!.content) as Delta;

    this.title.quillEditor.setContents(titleDelta);
    this.content.quillEditor.setContents(contentDeltas);
  }
  submit(){
    if (!this.content.quillEditor || !this.title.quillEditor) return;

    let titleText = this.title.quillEditor.getText();
    let contentText = this.content.quillEditor.getText();

    if (titleText.length < 3 || contentText.length < 3) return;

    let titleDelta = this.title.quillEditor.getContents() as Delta;
    let contentDeltas = this.content.quillEditor.getContents();

    let docComposedModel = {
      id: this.urlGuid!.toString(),
      title: titleText,
      titleDelta: JSON.stringify(titleDelta),
      content: contentText,
      contentDeltas: JSON.stringify(contentDeltas)
    }

    if (!isEqual(docComposedModel, this.fetchedDoc)) return;

    this.store.dispatch(docCrudActions.doc_save());
    this.store.dispatch(docCrudActions.doc_patch(docComposedModel));
  }
  showToolBar(selectionChange: SelectionChange){
    if (this.linkInputOpened && selectionChange.oldRange){
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

    let range = selectionChange.range;
    if (range !== null && range.length !==0 ){
      const selectedText = selectionChange.editor.getText(range.index, range.length);
      const selectedTextFormats = selectionChange.editor.getFormat(range.index, range.length);

      const selection = window.getSelection()!;
      const rangeRect = selection.getRangeAt(0).getBoundingClientRect();

      this.store.dispatch(quillSelectionActions.quill_newSelection({
        toolbarOpened: true,
        range: range,
        bounds: {left: rangeRect.x, top: rangeRect.y},
        text: selectedText,
        formats: selectedTextFormats,
        linkInputOpened: false
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
