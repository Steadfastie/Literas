import {Component, ElementRef, Input, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import {Subject, takeUntil} from "rxjs";
import {QuillState} from "../../../state/models/quill.state";
import * as quillSelectionsSelectors from "src/app/state/selectors/quill.selection.selectors";
import {IToolbarModel, ToolBarConfig} from "../../../models/quill/toolbar";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.sass']
})
export class ToolbarComponent implements OnInit, OnDestroy{
  @Input() editor!: QuillEditorComponent;
  currentSelection?: QuillState;
  active: IToolbarModel = ToolBarConfig;
  linkInputOpened: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private el: ElementRef,
              private renderer: Renderer2,
              private store: Store) {

    this.store.select(quillSelectionsSelectors.selectQuillState)
      .pipe(takeUntil(this.subManager$))
      .subscribe(selection => {
        this.currentSelection = selection;
      });

    this.store.select(quillSelectionsSelectors.selectCurrentSelectionFormats)
      .pipe(takeUntil(this.subManager$))
      .subscribe(formats => {
        if (Object.keys(formats).length !== 0){
          Object.keys(formats).forEach(format => {
            this.active[format] = formats[format];
          });
        } else {
          Object.keys(this.active).forEach(format => {
            this.active[format] = false;
          });
        }
      });

    this.store.select(quillSelectionsSelectors.selectLinkInputOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.linkInputOpened = status;
      });
  }
  setBold(){
    if (!this.currentSelection) return;
    if (this.active['code-block']) return;

    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'bold',
      !this.currentSelection.formats['bold']);

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
        {format: 'bold', value: !this.currentSelection.formats['bold']}
      )
    );
  }
  setItalic(){
    if (!this.currentSelection) return;
    if (this.active['code-block']) return;

    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'italic',
      !this.currentSelection.formats['italic']);

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
        {format: 'italic', value: !this.currentSelection.formats['italic']}
      )
    );
  }
  editLink(){
    if (!this.currentSelection) return;

    this.store.dispatch(quillSelectionActions.quill_switchLinkInput());
  }
  setCodeBlock(){
    if (!this.currentSelection) return;
    this.editor.quillEditor.formatLine(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'code-block',
      !this.currentSelection.formats['code-block']);

    this.store.dispatch(
      quillSelectionActions.quill_formatsChange(
        {formats: [
            {format: 'list', value: false},
            {format: 'code-block', value: !this.currentSelection.formats['code-block']}
          ]}
      )
    );
  }
  setHeader(){
    if (!this.currentSelection) return;
    if (this.active['code-block']) return;

    let updatedSize =
      this.currentSelection.formats['size'] === undefined ? 'large' :
      this.currentSelection.formats['size'] === false ? 'large' : false;

    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'size',
      updatedSize);

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
        {format: 'size', value: updatedSize}
      )
    );
  }
  setOrderedList(){
    if (!this.currentSelection) return;

    let updatedOrderedList =
      this.currentSelection.formats['list'] === undefined ? 'ordered' :
      this.currentSelection.formats['list'] === false ? 'ordered' :
      this.currentSelection.formats['list'] === 'bullet' ? 'ordered' : false;

    this.editor.quillEditor.formatLine(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'list',
      updatedOrderedList);

    this.store.dispatch(
      quillSelectionActions.quill_formatsChange(
        {formats: [
            {format: 'code-block', value: false},
            {format: 'list', value: updatedOrderedList},
          ]}
      )
    );
  }
  setBulletList(){
    if (!this.currentSelection) return;

    let updatedBulletList =
      this.currentSelection.formats['list'] === undefined ? 'bullet' :
      this.currentSelection.formats['list'] === false ? 'bullet' :
      this.currentSelection.formats['list'] === 'ordered' ? 'bullet' : false;

    this.editor.quillEditor.formatLine(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'list',
      updatedBulletList);

    this.store.dispatch(
      quillSelectionActions.quill_formatsChange(
        {formats: [
            {format: 'code-block', value: false},
            {format: 'list', value: updatedBulletList}
          ]}
      )
    );
  }
  ngOnInit(): void {

  }
  ngOnDestroy(): void {
    this.subManager$.next('unsubscribed');
  }
}
