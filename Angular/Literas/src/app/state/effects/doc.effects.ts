import { Injectable } from '@angular/core';
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {DocService} from "../../services/docs/doc.service";
import * as docCrudActions from "../actions/docs.crud.actions";
import {catchError, exhaustMap, map, of} from "rxjs";
import {IErrorModel} from "../../models/system/error.model";

@Injectable({
  providedIn: 'root'
})
export class DocsEffectsService {

  constructor(private actions$: Actions,
              private docService: DocService) { }

  fetchDocs$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.docs_fetch),
    exhaustMap(() => this.docService.getAllDocs()
      .pipe(
        map(docs => docCrudActions.docs_fetch_success({fetched: docs})),
        catchError(error => of(docCrudActions.docs_fetch_failed(error)))
      ))
  ))
}
