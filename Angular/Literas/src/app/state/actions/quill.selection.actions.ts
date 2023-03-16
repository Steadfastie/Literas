import {createAction, props} from "@ngrx/store";
import {IQuillState} from "../models/quill.state";
import {QuillFormat} from "../../models/quill/format";

export enum QuillSelectionActions{
  NewSelection = '[Quill] New Selection',
  FormatChange = '[Quill] Format Change',
  FormatsChange = '[Quill] Formats Change',
  FocusOff = '[Quill] Focus Off',
}

export const quill_newSelection = createAction(
  QuillSelectionActions.NewSelection,
  props<IQuillState>()
)

export const quill_formatChange = createAction(
  QuillSelectionActions.FormatChange,
  props<QuillFormat>()
)

export const quill_formatsChange = createAction(
  QuillSelectionActions.FormatsChange,
  props<{formats: QuillFormat[]}>()
)

export const quill_focusOff = createAction(
  QuillSelectionActions.FocusOff
)
