import {RangeStatic, StringMap} from "quill";
import {Bounds} from "../../models/quill/bounds";

export interface QuillState {
  range: RangeStatic | null;
  bounds: Bounds | null;
  text: string | null;
  formats: StringMap;
  linkInputOpenState: boolean;
}
