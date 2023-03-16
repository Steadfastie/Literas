import {IQuillState} from "../models/quill.state";
import {createReducer, on} from "@ngrx/store";
import * as quillSelectionActions from "../actions/quill.selection.actions";

export const quillInitialSelectionState: IQuillState = {
  range: null,
  text: '',
  formats: {},
}

export const quillSelectionReducer = createReducer(
  quillInitialSelectionState,
  on(quillSelectionActions.quill_newSelection,
    (state, selection: IQuillState) => ({...state, range: selection.range, text: selection.text, formats: selection.formats})
  ),
  on(quillSelectionActions.quill_formatChange,
    (state, {format, value}) => ({...state, formats: {...state.formats, [format]: value}})
  ),
  on(quillSelectionActions.quill_formatsChange,
    (state, { formats }) => {
      const newFormats = { ...state.formats };
      formats.forEach(format => {
        newFormats[format.format] = format.value;
      });
      return { ...state, formats: newFormats };
    }
  ),
  on(quillSelectionActions.quill_focusOff,
    () => (quillInitialSelectionState)
  )
)
