import {DocResponseModel} from "../../models/docs/docs.response.model";
import {ErrorModel} from "../../models/system/error.model";
import {DocThumbnail} from "../../models/docs/doc.thumbnail";

export interface DocsState {
  docThumbnails: DocThumbnail[]
  currentDocLastSave?: DocResponseModel,
  errors: ErrorModel[],
  urlId?: string,
  saving: boolean,
}
