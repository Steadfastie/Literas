import {Component, ElementRef, Input, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {QuillEditorComponent} from "ngx-quill";
import {Store} from "@ngrx/store";
import {Subject, takeUntil} from "rxjs";
import {IQuillState} from "../../../state/models/quill.state";
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
  currentSelection?: IQuillState;
  active: IToolbarModel = ToolBarConfig;
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
  }

  setBold(){
    if (!this.currentSelection) return;

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

  setLink(){

  }

  setCodeBlock(){
    if (!this.currentSelection) return;
    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'code-block',
      !this.currentSelection.formats['code-block']);

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
        {format: 'code-block', value: !this.currentSelection.formats['code-block']}
      )
    );
  }

  setHeader(){
    if (!this.currentSelection) return;
    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'size',
      !this.currentSelection.formats['size']);

    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
        {format: 'header', value: !this.currentSelection.formats['header']}
      )
    );
  }


  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.subManager$.next('unsubscribed');
  }
}
