import {DocsState} from "./docs.state";
import {QuillState} from "./quill.state";

export interface AppState {
  docs: DocsState
  quill: QuillState
}
