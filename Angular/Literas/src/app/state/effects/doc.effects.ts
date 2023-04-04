import { Injectable } from '@angular/core';
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {DocService} from "../../services/docs/doc.service";
import * as docCrudActions from "../actions/docs.crud.actions";
import {catchError, exhaustMap, map, of, switchMap, tap} from "rxjs";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class DocCrudEffects {

  constructor(private actions$: Actions,
              private docService: DocService,
              private router: Router) { }


  fetchDoc$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_fetch),
    exhaustMap((docId) => this.docService.getDocById(docId.id)
      .pipe(
        map(doc => docCrudActions.doc_fetch_success(doc)),
        catchError(error => of(docCrudActions.doc_fetch_failed(error)))
      ))
  ))

  fetchDocs$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.docs_fetch),
    exhaustMap(() => this.docService.getAllDocs()
      .pipe(
        map(docs => docCrudActions.docs_fetch_success({fetched: docs})),
        catchError(error => of(docCrudActions.docs_fetch_failed(error)))
      ))
  ))

  fetchDocThumbnails$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_thumbnails_fetch),
    exhaustMap(() => this.docService.getDocThumbnails()
      .pipe(
        map(thumbnails => docCrudActions.doc_thumbnails_fetch_success({fetched: thumbnails})),
        catchError(error => of(docCrudActions.doc_thumbnails_fetch_failed(error)))
    ))
  ))

  createDoc$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_create),
    exhaustMap((doc) => this.docService.create(doc)
      .pipe(
        map(docResponse => docCrudActions.doc_create_success(docResponse)),
        catchError(error => of(docCrudActions.doc_create_failed(error)))
      )
    )))

  createDocSuccess$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_create_success),
    tap((docResponse) => {
      setTimeout(() => {
        this.router.navigate([`/docs/${docResponse.id}`]);
      }, 0);
    })),
    {dispatch: false})

  patchDoc$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_patch),
    switchMap((doc) => this.docService.patch(doc)
      .pipe(
        map(docResponse => docCrudActions.doc_patch_success(docResponse)),
        catchError(error => of(docCrudActions.doc_patch_failed(error)))
      ))))

  deleteDoc$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_delete),
    exhaustMap((docId) => this.docService.delete(docId.id)
      .pipe(
        map(docResponse => docCrudActions.doc_delete_success(docResponse)),
        catchError(error => of(docCrudActions.doc_delete_failed(error)))
      ))))

  deleteDocSuccess$ = createEffect(() => this.actions$.pipe(
    ofType(docCrudActions.doc_delete_success),
    tap(() => {
      setTimeout(() => {
        this.router.navigate(['/docs']);
      }, 0);
    })),
    {dispatch: false})
}
