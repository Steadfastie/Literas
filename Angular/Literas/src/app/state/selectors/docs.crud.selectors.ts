import {createFeatureSelector, createSelector} from "@ngrx/store";
import {DocsState} from "../models/docs.state";

export const selectWholeDocsCrudState = createFeatureSelector<DocsState>('docs_crud');

export const selectCurrentDocLastSave = createSelector(
  selectWholeDocsCrudState,
  (state) => state.currentDocLastSave
)

export const selectDocThumbnails = createSelector(
  selectWholeDocsCrudState,
  state => state.docThumbnails
)

export const selectErrorsInDocCrudState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.errors
)

export const selectSavingState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.saving
)
