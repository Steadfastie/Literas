import {Component, OnDestroy} from '@angular/core';
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import * as docSelectors from "../../../state/selectors/docs.crud.selectors";
import {Store} from "@ngrx/store";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'save-toggle',
  templateUrl: './save.toggle.component.html',
  styleUrls: ['./save.toggle.component.sass']
})
export class SaveToggleComponent implements OnDestroy{
  isSaving: boolean = false
  subManager$: Subject<any> = new Subject();
  constructor(private store: Store) {
    this.store.select(docSelectors.selectSavingState)
      .pipe(takeUntil(this.subManager$))
      .subscribe((saving) => {
      this.isSaving = saving
    })
  }
  saveCurrentDoc() {
    this.store.dispatch(docCrudActions.doc_save())
  }

  ngOnDestroy(): void {
    this.subManager$.next('destroyed')
  }
}
