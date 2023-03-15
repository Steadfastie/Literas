import {Component, ElementRef, Input, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {QuillEditorComponent} from "ngx-quill";
import {RangeStatic, StringMap} from "quill";
import {Store} from "@ngrx/store";
import {Subject, takeUntil} from "rxjs";
import {IQuillState} from "../../../state/models/quill.state";
import * as quillSelectionsSelectors from "src/app/state/selectors/quill.selection.selectors";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.sass']
})
export class ToolbarComponent implements OnInit, OnDestroy{
  @Input() editor!: QuillEditorComponent;
  currentSelection?: IQuillState;
  lastSelectionFormats: StringMap = {};
  subManager$: Subject<any> = new Subject();
  constructor(private el: ElementRef,
              private renderer: Renderer2,
              private store: Store) {
    this.store.select(quillSelectionsSelectors.selectQuillState)
      .pipe(takeUntil(this.subManager$))
      .subscribe(selection => {
        this.currentSelection = selection;
        if (Object.keys(this.lastSelectionFormats).length !== 0){
          Object.keys(this.lastSelectionFormats).forEach(format => {
            this.activateButton(`${format}Button`);
            delete this.lastSelectionFormats[format];
          });
        }
      });
    this.store.select(quillSelectionsSelectors.selectCurrentSelectionFormats)
      .pipe(takeUntil(this.subManager$))
      .subscribe(formats => {
        this.lastSelectionFormats = formats;
        if (Object.keys(formats).length !== 0){
          Object.keys(formats).forEach(format => {
            this.activateButton(`${format}Button`)
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
    this.activateButton('boldButton');
  }
  setItalic(){
    if (!this.currentSelection) return;
    this.editor.quillEditor.formatText(
      this.currentSelection.range!.index,
      this.currentSelection.range!.length,
      'italic',
      !this.currentSelection.formats['italic']);
    this.activateButton('italicButton');
  }

  ngOnInit(): void {

  }

  activateButton(className: string){
    const toolButton = this.el.nativeElement.querySelector(`.${className}`);
    if (toolButton.classList.contains('active')) {
      this.renderer.removeClass(toolButton, 'active');
    } else {
      this.renderer.addClass(toolButton, 'active');
    }
  }

  ngOnDestroy(): void {
    this.subManager$.next('unsubscribed');
  }
}
