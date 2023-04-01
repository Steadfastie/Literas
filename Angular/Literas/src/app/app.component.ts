import { Component } from '@angular/core';
import {Store} from "@ngrx/store";
import * as docCrudActions from './state/actions/docs.crud.actions';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'Literas';

  constructor(private store: Store) {}

  saveCurrentDoc() {
    this.store.dispatch(docCrudActions.doc_save())
  }
}
