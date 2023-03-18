import {RangeStatic, StringMap} from "quill";

export interface IQuillState {
  range: RangeStatic | null;
  text: string;
  formats: StringMap;
  linkInputOpenState: boolean;
}
