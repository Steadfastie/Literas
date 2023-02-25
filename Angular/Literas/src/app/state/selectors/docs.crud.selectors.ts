import {createFeatureSelector, createSelector} from "@ngrx/store";
import {IDocsState} from "../models/docs.state";

export const docsCrudFeatureKey = 'docs_crud';

export const selectWholeDocsCrudState = createFeatureSelector<IDocsState>(docsCrudFeatureKey);

export const selectDocsCrudState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.docs
)

export const selectErrorsInDocCrudState = createSelector(
  selectWholeDocsCrudState,
  (state) => state.errors
)
