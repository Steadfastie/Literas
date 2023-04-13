import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {DocService} from "../../../services/docs/doc.service";
import {QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import { SelectionChange} from "ngx-quill/lib/quill-editor.component";
import * as quillSelectionsSelectors from "../../../state/selectors/quill.selection.selectors";
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import {debounceTime, distinctUntilChanged, filter, Subject, takeUntil} from "rxjs";
import * as quillSelectionSelectors from "../../../state/selectors/quill.selection.selectors";
import {SelectionService} from "../../../services/quill/selection.service";
import {SelectionFraudService} from "../../../services/quill/selection.fraud.service";
import {DocSubmitService} from "../../../services/docs/doc.submit.service";
import {SaveToggleService} from "../../../services/header/save.toggle.service";

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
  subManager$ = new Subject<void>();
  constructor(private docService: DocService,
              private docSubmitService: DocSubmitService,
              private selectionService: SelectionService,
              private selectionFraudService: SelectionFraudService,
              private saveToggleService: SaveToggleService,
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

    this.docSubmitService.submit.next({
      title: this.title.quillEditor.getText(),
      titleDelta: this.title.quillEditor.getContents(),
      content: this.content.quillEditor.getText(),
      contentDelta: this.content.quillEditor.getContents(),
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
  ngOnInit(): void {
    this.store.select(quillSelectionsSelectors.selectLinkInputOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.linkInputOpened = status;
      });

    this.saveToggleService.manualSave
      .pipe(takeUntil(this.subManager$))
      .subscribe(() => {
        this.saved = true;
        this.submit();
      });

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
    this.subManager$.next();
    this.subManager$.complete();
  }
}
