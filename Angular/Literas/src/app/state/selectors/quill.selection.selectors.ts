import {QuillState} from "../models/quill.state";
import {createFeatureSelector, createSelector} from "@ngrx/store";

export const selectQuillState = createFeatureSelector<QuillState>('quill');

export const selectCurrentRange = createSelector(
  selectQuillState,
  (state) => state.range
)

export const selectCurrentSelectionText = createSelector(
  selectQuillState,
  (state) => state.text
)

export const selectCurrentSelectionFormats = createSelector(
  selectQuillState,
  (state) => state.formats
)

export const selectLinkInputOpenState = createSelector(
  selectQuillState,
  (state) => state.linkInputOpenState
)
