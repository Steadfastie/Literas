import {IDocsState} from "../models/docs.state";
import {createReducer, on} from "@ngrx/store";
import * as docsCrudActions from "../actions/docs.crud.actions";
import {docs_fetch} from "../actions/docs.crud.actions";
import {IDocResponseModel} from "../../models/docs/docs.response.model";

export const docsInitialState: IDocsState = {
  docs: [],
  errors: []
}

export const docsReducer = createReducer(
  docsInitialState,
  on(docsCrudActions.docs_fetch_success, (state, docs) => ({...state, docs: docs.fetched})),
  on(docsCrudActions.docs_fetch_failed, (state, error) => ({...state, error: [...state.errors, error]})),
  on(docsCrudActions.docs_load_success, (state, doc) => ({...state, docs: [...state.docs, doc]})),
  on(docsCrudActions.docs_load_failed, (state, error) => ({...state, error: [...state.errors, error]})),
)


