import {createAction, props} from "@ngrx/store";
import {QuillState} from "../models/quill.state";
import {QuillFormat} from "../../models/quill/format";

export enum QuillSelectionActions{
  NewSelection = '[Quill] New Selection',
  FormatChange = '[Quill] Format Change',
  FormatsChange = '[Quill] Formats Change',
  SwitchLinkInput = '[Quill] Switch Link Input',
  FocusOff = '[Quill] Focus Off',
}

export const quill_newSelection = createAction(
  QuillSelectionActions.NewSelection,
  props<QuillState>()
)

export const quill_formatChange = createAction(
  QuillSelectionActions.FormatChange,
  props<QuillFormat>()
)

export const quill_formatsChange = createAction(
  QuillSelectionActions.FormatsChange,
  props<{formats: QuillFormat[]}>()
)

export const quill_switchLinkInput = createAction(
  QuillSelectionActions.SwitchLinkInput,
)

export const quill_focusOff = createAction(
  QuillSelectionActions.FocusOff
)
