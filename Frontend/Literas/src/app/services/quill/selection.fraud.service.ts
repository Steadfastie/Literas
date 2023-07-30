import {Injectable, OnDestroy} from '@angular/core';
import Quill from "quill";
import {Store} from "@ngrx/store";
import * as quillSelectionSelectors from 'src/app/state/selectors/quill.selection.selectors';
import {Subject, takeUntil} from "rxjs";
import {Range} from "../../models/quill/range";

@Injectable({
  providedIn: 'root'
})
export class SelectionFraudService implements OnDestroy{
  toolbarOpened: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private store: Store) {
    this.store.select(quillSelectionSelectors.selectToolbarOpened)
      .pipe(takeUntil(this.subManager$))
      .subscribe(toolbarOpened => {
        this.toolbarOpened = toolbarOpened;
      });
  }
  colorizeSelection(editor: Quill, range: Range){
    if (!this.toolbarOpened) return;
    editor.formatText(
      range.index,
      range.length,
      'background', '#338dfa'
    );
    editor.formatText(
      range.index,
      range.length,
      'color', 'white'
    );
  }
  stripSelection(editor: Quill, range: Range){
    if (!this.toolbarOpened) return;
    editor.formatText(
      range.index,
      range.length,
      'background', ''
    );
    editor.formatText(
      range.index,
      range.length,
      'color', ''
    );
  }
  ngOnDestroy(): void {
    this.subManager$.next('destroyed');
  }
}
