import {BoundsStatic, Quill, RangeStatic, StringMap} from "quill";
import {QuillEditorComponent} from "ngx-quill";
import {Bounds} from "../../models/quill/bounds";

export interface IQuillState {
  range: RangeStatic | null;
  bounds: Bounds | null;
  text: string | null;
  formats: StringMap;
  linkInputOpenState: boolean;
}
