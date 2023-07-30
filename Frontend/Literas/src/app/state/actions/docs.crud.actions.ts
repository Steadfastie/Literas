import {createAction, props} from "@ngrx/store";
import {DocRequestModel} from "../../models/docs/docs.request.model";
import {DocResponseModel} from "../../models/docs/docs.response.model";
import {ErrorModel} from "../../models/system/error.model";
import {DocThumbnail} from "../../models/docs/doc.thumbnail";

export enum DocsCrudActions{
  FetchDocs = '[Docs] Docs fetch initialized',
  FetchDocsSuccess = '[Docs] Docs fetch successful',
  FetchDocsFailed = '[Docs] Docs fetch failure',

  FetchDoc = '[Docs] Doc fetch initialized',
  FetchDocSuccess = '[Docs] Doc fetch successful',
  FetchDocFailed = '[Docs] Doc fetch failure',

  CreateDoc = '[Docs] Doc creation initialized',
  CreateDocSuccess = '[Docs] Doc creation successful',
  CreateDocFailed = '[Docs] Doc creation failed',

  FetchDocThumbnails = '[Docs] Doc thumbnails fetch initialized',
  FetchDocThumbnailsSuccess = '[Docs] Doc thumbnails fetch successful',
  FetchDocThumbnailsFailed = '[Docs] Doc thumbnails fetch failure',

  UrlIdChange = '[Docs] Url id change',
  SaveDoc = '[Docs] Doc saving initialized',

  PatchDoc = '[Docs] Doc patching initialized',
  PatchDocSuccess = '[Docs] Doc patching successful',
  PatchDocFailed = '[Docs] Doc patching failed',

  ClearLastSave = '[Docs] Clear last save',

  DeleteDoc = '[Docs] Doc deletion initialized',
  DeleteDocSuccess = '[Docs] Doc deletion successful',
  DeleteDocFailed = '[Docs] Doc deletion failed',
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

export const doc_fetch = createAction(
  DocsCrudActions.FetchDoc,
  props<{id: string}>()
)
export const doc_fetch_success = createAction(
  DocsCrudActions.FetchDocSuccess,
  props<DocResponseModel>()
)
export const doc_fetch_failed = createAction(
  DocsCrudActions.FetchDocFailed,
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

export const url_id_change = createAction(
  DocsCrudActions.UrlIdChange,
  props<{ id?: string }>()
)
export const doc_save = createAction(
  DocsCrudActions.SaveDoc
)

export const doc_patch = createAction(
  DocsCrudActions.PatchDoc,
  props<DocRequestModel>()
)
export const doc_patch_success = createAction(
  DocsCrudActions.PatchDocSuccess,
  props<DocResponseModel>()
)
export const doc_patch_failed = createAction(
  DocsCrudActions.PatchDocFailed,
  props<ErrorModel>()
)

export const doc_clear_last_save = createAction(
  DocsCrudActions.ClearLastSave
)

export const doc_delete = createAction(
  DocsCrudActions.DeleteDoc,
  props<{id: string}>()
)
export const doc_delete_success = createAction(
  DocsCrudActions.DeleteDocSuccess,
  props<DocResponseModel>()
)
export const doc_delete_failed = createAction(
  DocsCrudActions.DeleteDocFailed,
  props<ErrorModel>()
)



