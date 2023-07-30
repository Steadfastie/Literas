import {Delta} from "quill";
import {DocResponseModel} from "./docs.response.model";

export interface DocScaffold {
  title: string;
  titleDelta: Delta;
  content: string;
  contentDelta: Delta;
  fetchedDoc?: DocResponseModel;
}
