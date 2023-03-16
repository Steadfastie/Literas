import {createAction, props} from "@ngrx/store";
import {IQuillState} from "../models/quill.state";

export enum QuillSelectionActions{
  NewSelection = '[Quill] New Selection',
  FormatChange = '[Quill] Format Change',
  FocusOff = '[Quill] Focus Off',
}

export const quill_newSelection = createAction(
  QuillSelectionActions.NewSelection,
  props<IQuillState>()
)

export const quill_formatChange = createAction(
  QuillSelectionActions.FormatChange,
  props<{format: string, value: boolean}>()
)

export const quill_focusOff = createAction(
  QuillSelectionActions.FocusOff
)
