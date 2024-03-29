import {QuillState} from "../models/quill.state";
import {createReducer, on} from "@ngrx/store";
import * as quillSelectionActions from "../actions/quill.selection.actions";

export const quillInitialSelectionState: QuillState = {
  toolbarOpened: false,
  range: null,
  bounds: null,
  text: null,
  formats: {},
  linkInputOpened: false
}

export const quillSelectionReducer = createReducer(
  quillInitialSelectionState,
  on(quillSelectionActions.quill_newSelection,
    (state, selection: QuillState) => (
      {...state,
        toolbarOpened: selection.toolbarOpened,
        range: selection.range,
        bounds: selection.bounds,
        text: selection.text,
        formats: selection.formats,
        linkInputOpened: false
      })
  ),
  on(quillSelectionActions.quill_formatChange,
    (state, {format, value}) => (
      {...state,
        formats: {...state.formats, [format]: value}
      })
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
  on(quillSelectionActions.quill_switchLinkInput,
    (state) => ({...state, linkInputOpened: !state.linkInputOpened})
  ),
  on(quillSelectionActions.quill_focusOff,
    () => (quillInitialSelectionState)
  )
)
