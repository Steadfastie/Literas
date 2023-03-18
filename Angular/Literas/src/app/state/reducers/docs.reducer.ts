import {IDocsState} from "../models/docs.state";
import {createReducer, on} from "@ngrx/store";
import * as docsCrudActions from "../actions/docs.crud.actions";

export const docsInitialState: IDocsState = {
  doc_thumbnails: [],
  docs: [],
  errors: [],
}

export const docsReducer = createReducer(
  docsInitialState,
  on(docsCrudActions.docs_fetch_success,
    (state, docs) => ({...state, docs: docs.fetched})),
  on(docsCrudActions.docs_fetch_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),
  on(docsCrudActions.docs_load_success,
    (state, doc) => ({...state, docs: [...state.docs, doc]})),
  on(docsCrudActions.docs_load_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),
  on(docsCrudActions.doc_thumbnails_fetch_success,
    (state, thumbnails) => ({...state, doc_thumbnails: [...state.doc_thumbnails, ...thumbnails.fetched]})),
  on(docsCrudActions.doc_thumbnails_fetch_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),
)


