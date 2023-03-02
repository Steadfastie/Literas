import {IDocResponseModel} from "../../models/docs/docs.response.model";
import {IErrorModel} from "../../models/system/error.model";
import {IDocThumbnail} from "../../models/docs/doc.thumbnail";

export interface IDocsState{
  doc_thumbnails: IDocThumbnail[]
  docs: IDocResponseModel[],
  errors: IErrorModel[]
}
