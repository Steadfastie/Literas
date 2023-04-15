import {Injectable, OnDestroy, OnInit} from '@angular/core';
import {Subject} from "rxjs";
import {Store} from "@ngrx/store";
import {Range} from "../../models/quill/range";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import Quill from "quill";

@Injectable({
  providedIn: 'root'
})
export class SelectionService implements OnInit, OnDestroy{
  subManager$: Subject<any> = new Subject();
  constructor(private store: Store) { }
  ngOnInit(): void {
  }
  setSelection(editor: Quill, range: Range | null): void {
    if (range !== null && range.length !==0){
      const selectedText = editor.getText(range.index, range.length);
      const selectedTextFormats = editor.getFormat(range.index, range.length);

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
    this.subManager$.next('destroyed');
    this.subManager$.complete();
  }

}
