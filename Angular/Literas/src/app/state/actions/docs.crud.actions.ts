import {createAction, props} from "@ngrx/store";
import {IDocRequestModel} from "../../models/docs/docs.request.model";
import {IDocResponseModel} from "../../models/docs/docs.response.model";
import {IErrorModel} from "../../models/system/error.model";
import {IDocThumbnail} from "../../models/docs/doc.thumbnail";

export enum DocsCrudActions{
  FetchDocs = '[Docs] Doc fetch initialized',
  FetchDocsSuccess = '[Docs] Doc fetch successful',
  FetchDocsFailed = '[Docs] Doc fetch failure',
  LoadDoc = '[Docs] Doc load initialized',
  LoadDocSuccess = '[Docs] Doc load successful',
  LoadDocFailed = '[Docs] Doc load failure',
  CreateDoc = '[Docs] Doc creation initialized',
  CreateDocSuccess = '[Docs] Doc creation successful',
  CreateDocFailed = '[Docs] Doc creation failed',
  FetchDocThumbnails = '[Docs] Doc thumbnails fetch initialized',
  FetchDocThumbnailsSuccess = '[Docs] Doc thumbnails fetch successful',
  FetchDocThumbnailsFailed = '[Docs] Doc thumbnails fetch failure',
}

export const docs_fetch = createAction(
  DocsCrudActions.FetchDocs
)

export const docs_fetch_success = createAction(
  DocsCrudActions.FetchDocsSuccess,
  props<{fetched: IDocResponseModel[]}>()
)

export const docs_fetch_failed = createAction(
  DocsCrudActions.FetchDocsFailed,
  props<IErrorModel>()
)

export const docs_load = createAction(
  DocsCrudActions.LoadDoc,
  props<{id: string}>()
)

export const docs_load_success = createAction(
  DocsCrudActions.LoadDocSuccess,
  props<IDocResponseModel>()
)

export const docs_load_failed = createAction(
  DocsCrudActions.LoadDocFailed,
  props<IErrorModel>()
)

export const doc_create = createAction(
  DocsCrudActions.CreateDoc,
  props<IDocRequestModel>()
)

export const doc_create_success = createAction(
  DocsCrudActions.CreateDocSuccess,
  props<IDocResponseModel>()
)

export const doc_create_failed = createAction(
  DocsCrudActions.CreateDocFailed,
  props<IErrorModel>()
)

export const doc_thumbnails_fetch = createAction(
  DocsCrudActions.FetchDocThumbnails
)

export const doc_thumbnails_fetch_success = createAction(
  DocsCrudActions.FetchDocThumbnailsSuccess,
  props<{fetched: IDocThumbnail[]}>()
)

export const doc_thumbnails_fetch_failed = createAction(
  DocsCrudActions.FetchDocThumbnailsFailed,
  props<IErrorModel>()
)



