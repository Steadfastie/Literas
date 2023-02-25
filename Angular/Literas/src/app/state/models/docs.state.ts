import {IDocResponseModel} from "../../models/docs/docs.response.model";
import {IErrorModel} from "../../models/system/i.error.model";

export interface IDocsState{
  docs: IDocResponseModel[],
  errors: IErrorModel[]
}
