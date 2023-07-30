import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {Store} from "@ngrx/store";
import * as docCrudActions from "src/app/state/actions/docs.crud.actions";
import {isEqual} from "lodash";
import {DocScaffold} from "../../models/docs/doc.scaffold";
import {DocRequestModel} from "../../models/docs/docs.request.model";
import {DocResponseModel} from "../../models/docs/docs.response.model";
import {Guid} from "guid-typescript";

@Injectable({
  providedIn: 'root'
})
export class DocSubmitService {
  public submit = new Subject<DocScaffold>();
  constructor(private store: Store) {
    this.submit.subscribe(scaffold => {
      if (!this.validateModel(scaffold)) return;

      let newModel = this.composeModel(scaffold);

      if (scaffold.fetchedDoc) {
        this.update(newModel, scaffold.fetchedDoc)
      } else {
        this.create(newModel);
      }
    });
  }
  validateModel(scaffold: DocScaffold): boolean{
    if (scaffold.title.length <= 3 || scaffold.content.length <= 3) return false;
    if (scaffold.titleDelta.ops?.length !== 1) return false;
    if (scaffold.contentDelta.ops?.length! < 1) return false;

    return true;
  }
  composeModel(scaffold: DocScaffold): DocRequestModel{
    return {
      id: scaffold.fetchedDoc ? scaffold.fetchedDoc.id : Guid.create().toString(),
      title: scaffold.title,
      titleDelta: JSON.parse(JSON.stringify(scaffold.titleDelta)),
      content: scaffold.content,
      contentDelta: JSON.parse(JSON.stringify(scaffold.contentDelta))
    }
  }
  create(model: DocRequestModel){
    this.store.dispatch(docCrudActions.doc_save());
    this.store.dispatch(docCrudActions.doc_create(model));
  }
  update(newModel: DocRequestModel, oldModel: DocResponseModel){
    if (isEqual(newModel, oldModel)) return;

    this.store.dispatch(docCrudActions.doc_save());
    this.store.dispatch(docCrudActions.doc_patch(newModel));
  }
}
