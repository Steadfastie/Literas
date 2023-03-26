import {BoundsStatic, RangeStatic, StringMap} from "quill";

export interface IQuillState {
  range: RangeStatic | null;
  bounds: BoundsStatic | {left: number, top: number}| null;
  text: string;
  formats: StringMap;
  linkInputOpenState: boolean;
}
