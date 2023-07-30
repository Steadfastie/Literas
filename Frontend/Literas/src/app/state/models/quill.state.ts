import QuillType, {RangeStatic, StringMap} from "quill";
import {Bounds} from "../../models/quill/bounds";

export interface QuillState {
  toolbarOpened: boolean;
  range: RangeStatic | null;
  bounds: Bounds | null;
  text: string | null;
  formats: StringMap;
  linkInputOpened: boolean;
}
