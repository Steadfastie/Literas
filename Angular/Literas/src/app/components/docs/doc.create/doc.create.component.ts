import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {DocService} from "../../../services/docs/doc.service";
import {QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import { SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import * as quillSelectionsSelectors from "../../../state/selectors/quill.selection.selectors";
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import * as docCrudSelectors from "../../../state/selectors/docs.crud.selectors";
import {debounceTime, distinctUntilChanged, filter, Subject, takeUntil} from "rxjs";
import {Guid} from "guid-typescript";
import {Delta} from "quill";
import * as quillSelectionSelectors from "../../../state/selectors/quill.selection.selectors";

@Component({
  selector: 'doc-create',
  templateUrl: './doc.create.component.html',
  styleUrls: ['./doc.create.component.sass']
})
export class DocCreateComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('titleQuill', {static: true}) title!: QuillEditorComponent;
  @ViewChild('contentQuill', {static: true}) content!: QuillEditorComponent;
  linkInputOpened: boolean = false;
  toolbarOpened: boolean = false;
  saved: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private docService: DocService,
              private el: ElementRef,
              private store: Store){
    this.store.select(quillSelectionSelectors.selectToolbarOpened)
      .pipe(
        filter(() => !this.saved),
        takeUntil(this.subManager$))
      .subscribe(toolbarOpened => {
        this.toolbarOpened = toolbarOpened
      });
  }
  submit(){
    if (!this.content?.quillEditor || !this.title?.quillEditor) return;

    let titleText = this.title.quillEditor.getText();
    let contentText = this.content.quillEditor.getText();

    if (titleText.length < 3 || contentText.length < 3) return;

    let titleDelta = this.title.quillEditor.getContents() as Delta;
    let contentDeltas = this.content.quillEditor.getContents();

    let docComposedModel = {
      id: Guid.create().toString(),
      title: titleText,
      titleDelta: JSON.parse(JSON.stringify(titleDelta)),
      content: contentText,
      contentDeltas: JSON.parse(JSON.stringify(contentDeltas))
    }

    this.saved = true;
    this.store.dispatch(docCrudActions.doc_create(docComposedModel));
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
  ngOnInit(): void {
    this.store.select(quillSelectionsSelectors.selectLinkInputOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.linkInputOpened = status;
      });

    this.store.select(docCrudSelectors.selectSavingState)
      .pipe(takeUntil(this.subManager$))
      .subscribe((saving) => {
        if (saving){
          this.saved = true;
          this.submit();
        }
      })

    this.title.onContentChanged
      .pipe(
        filter(() => !this.toolbarOpened && this.saved),
        takeUntil(this.subManager$),
        debounceTime(5000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        if (!this.toolbarOpened && !this.saved) this.submit();
      });

    this.content.onContentChanged
      .pipe(
        filter(() => !this.toolbarOpened && this.saved),
        takeUntil(this.subManager$),
        debounceTime(5000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.submit();
      });
  }
  ngAfterViewInit(): void {
    this.content!.elementRef.nativeElement.addEventListener('click', (event: Event) => {
      const target = event.target as HTMLElement;

      if (target.tagName === 'A') {
        event.preventDefault();
        window.open(target.getAttribute('href')!, '_blank');
      }
    });

    this.store.dispatch(docCrudActions.url_id_change({id: undefined}));
  }
  ngOnDestroy(): void {
    if (!this.saved) this.submit();
    this.subManager$.next('destroyed');
  }
}
