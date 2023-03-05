import {createFeatureSelector, createSelector} from "@ngrx/store";
import {IDocsState} from "../models/docs.state";

export const docsCrudFeatureKey = 'docs_crud';

export const selectWholeDocsCrudState = createFeatureSelector<IDocsState>('docs_crud');

export const selectDocsCrudState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.docs
)

export const selectDocThumbnails = createSelector(
  selectWholeDocsCrudState,
  state => state.doc_thumbnails
)

export const selectErrorsInDocCrudState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.errors
)
