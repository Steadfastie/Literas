import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {QuillEditorComponent} from "ngx-quill";
import {debounceTime, distinctUntilChanged, filter, skip, Subject, takeUntil} from "rxjs";
import {SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import * as quillSelectionSelectors from 'src/app/state/selectors/quill.selection.selectors';
import * as docSelectors from 'src/app/state/selectors/docs.crud.selectors';
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import {Store} from "@ngrx/store";
import {DocResponseModel} from "../../../models/docs/docs.response.model";
import {ActivatedRoute} from "@angular/router";
import {Guid} from "guid-typescript";
import {SelectionFraudService} from "../../../services/quill/selection.fraud.service";
import {SelectionService} from "../../../services/quill/selection.service";
import {DocSubmitService} from "../../../services/docs/doc.submit.service";
import {SaveToggleService} from "../../../services/header/save.toggle.service";

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
  subManager$ = new Subject<void>();
  constructor(private store: Store,
              private activatedRoute: ActivatedRoute,
              private selectionService: SelectionService,
              private selectionFraudService: SelectionFraudService,
              private docSubmitService: DocSubmitService,
              private saveToggleService: SaveToggleService) {
    this.store.select(quillSelectionSelectors.selectToolbarOpened)
      .pipe(
        skip(1),
        takeUntil(this.subManager$))
      .subscribe(toolbarOpened => {
        this.toolbarOpened = toolbarOpened
        if (!toolbarOpened) this.submit();
      });
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
        if (this.toolbarOpened){
          this.store.dispatch(quillSelectionActions.quill_focusOff());
        }
      }
    });

    this.saveToggleService.manualSave
      .pipe(takeUntil(this.subManager$))
      .subscribe(() => {
      this.submit();
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
        filter(() => !this.toolbarOpened),
        takeUntil(this.subManager$),
        debounceTime(1000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.submit();
      });

    this.title.onContentChanged
      .pipe(
        skip(1),
        filter(() => !this.toolbarOpened),
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

    this.title.quillEditor.setContents(this.fetchedDoc!.titleDelta);
    this.content.quillEditor.setContents(this.fetchedDoc!.contentDeltas);
  }
  submit(){
    if (!this.content.quillEditor || !this.title.quillEditor) return;
    if (this.urlGuid?.toString() != this.fetchedDoc?.id as string) return;

    this.docSubmitService.submit.next({
      title: this.title.quillEditor.getText(),
      titleDelta: this.title.quillEditor.getContents(),
      content: this.content.quillEditor.getText(),
      contentDelta: this.content.quillEditor.getContents(),
      fetchedDoc: this.fetchedDoc!
    });
  }
  showToolBar(selectionChange: SelectionChange){
    if (this.linkInputOpened && selectionChange.oldRange){
      this.selectionFraudService.colorizeSelection(
        selectionChange.editor, selectionChange.oldRange);
      return;
    }
    this.selectionService.setSelection(selectionChange.editor, selectionChange.range);
  }
  ngOnDestroy(): void {
    this.submit();
    this.subManager$.next();
    this.subManager$.complete();
    this.store.dispatch(docCrudActions.doc_clear_last_save());
    this.store.dispatch(docCrudActions.url_id_change({id: undefined}));
  }
}
