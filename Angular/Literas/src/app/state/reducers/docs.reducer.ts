import {DocsState} from "../models/docs.state";
import {createReducer, on} from "@ngrx/store";
import * as docsCrudActions from "../actions/docs.crud.actions";

export const docsInitialState: DocsState = {
  docThumbnails: [],
  currentDocLastSave: undefined,
  errors: [],
  urlId: undefined,
  saving: false,
}

export const docsReducer = createReducer(
  docsInitialState,

  on(docsCrudActions.doc_fetch_success,
    (state, doc) => ({...state, currentDocLastSave: doc})),
  on(docsCrudActions.doc_fetch_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),

  on(docsCrudActions.docs_fetch_success,
    (state, docs) => ({...state, docs: docs.fetched})),
  on(docsCrudActions.docs_fetch_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),

  on(docsCrudActions.doc_thumbnails_fetch_success,
    (state, thumbnails) => ({...state, docThumbnails: [...state.docThumbnails, ...thumbnails.fetched]})),
  on(docsCrudActions.doc_thumbnails_fetch_failed,
    (state, error) => ({...state, errors: [...state.errors, error]})),

  on(docsCrudActions.url_id_change,
    (state, urlId) => ({...state, urlId: urlId.id})),
  on(docsCrudActions.doc_save,
    (state) => ({...state, saving: true})),

  on(docsCrudActions.doc_create_success,
    (state, docResponse) => ({
      ...state,
      currentDocLastSave: docResponse,
      saving: false,
      docThumbnails: [
        ...state.docThumbnails.filter(thumbnail => thumbnail.id !== docResponse.id),
        {id: docResponse.id, title: docResponse.title}]
    })),
  on(docsCrudActions.doc_create_failed,
    (state, error) => ({
      ...state,
      errors: [...state.errors, error],
      saving: false
    })),

  on(docsCrudActions.doc_patch_success,
    (state, docResponse) => ({
      ...state,
      currentDocLastSave: docResponse,
      saving: false,
      docThumbnails: state.docThumbnails.map(
        thumbnail => thumbnail.id === docResponse.id && thumbnail.title !== docResponse.title
          ? { ...thumbnail, title: docResponse.title }
          : thumbnail
      )
    })),
  on(docsCrudActions.doc_patch_failed,
    (state, error) => ({
      ...state,
      errors: [...state.errors, error],
      saving: false
    })),
)


