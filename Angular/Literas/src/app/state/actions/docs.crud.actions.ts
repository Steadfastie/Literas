import {createAction, props} from "@ngrx/store";
import {DocRequestModel} from "../../models/docs/docs.request.model";
import {DocResponseModel} from "../../models/docs/docs.response.model";
import {ErrorModel} from "../../models/system/error.model";
import {DocThumbnail} from "../../models/docs/doc.thumbnail";

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
  SaveDoc = '[Docs] Doc saving initialized',
}

export const docs_fetch = createAction(
  DocsCrudActions.FetchDocs
)

export const docs_fetch_success = createAction(
  DocsCrudActions.FetchDocsSuccess,
  props<{fetched: DocResponseModel[]}>()
)

export const docs_fetch_failed = createAction(
  DocsCrudActions.FetchDocsFailed,
  props<ErrorModel>()
)

export const docs_load = createAction(
  DocsCrudActions.LoadDoc,
  props<{id: string}>()
)

export const docs_load_success = createAction(
  DocsCrudActions.LoadDocSuccess,
  props<DocResponseModel>()
)

export const docs_load_failed = createAction(
  DocsCrudActions.LoadDocFailed,
  props<ErrorModel>()
)

export const doc_create = createAction(
  DocsCrudActions.CreateDoc,
  props<DocRequestModel>()
)

export const doc_create_success = createAction(
  DocsCrudActions.CreateDocSuccess,
  props<DocResponseModel>()
)

export const doc_create_failed = createAction(
  DocsCrudActions.CreateDocFailed,
  props<ErrorModel>()
)

export const doc_thumbnails_fetch = createAction(
  DocsCrudActions.FetchDocThumbnails
)

export const doc_thumbnails_fetch_success = createAction(
  DocsCrudActions.FetchDocThumbnailsSuccess,
  props<{fetched: DocThumbnail[]}>()
)

export const doc_thumbnails_fetch_failed = createAction(
  DocsCrudActions.FetchDocThumbnailsFailed,
  props<ErrorModel>()
)

export const doc_save = createAction(
  DocsCrudActions.SaveDoc
)



