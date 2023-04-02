import { Component } from '@angular/core';
import * as docCrudActions from "../../../state/actions/docs.crud.actions";
import * as docSelectors from "../../../state/selectors/docs.crud.selectors";
import {Store} from "@ngrx/store";

@Component({
  selector: 'save-toggle',
  templateUrl: './save.toggle.component.html',
  styleUrls: ['./save.toggle.component.sass']
})
export class SaveToggleComponent {
  isSaving: boolean = false
  constructor(private store: Store) {
    this.store.select(docSelectors.selectSavingState)
      .subscribe((saving) => {
      this.isSaving = saving
    })
  }
  saveCurrentDoc() {
    this.store.dispatch(docCrudActions.doc_save())
  }
}
