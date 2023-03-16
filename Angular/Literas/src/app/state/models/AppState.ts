import {IDocsState} from "./docs.state";
import {IQuillState} from "./quill.state";

export interface AppState{
  docs: IDocsState
  quill: IQuillState
}
