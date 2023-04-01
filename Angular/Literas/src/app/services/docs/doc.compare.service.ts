import { Injectable } from '@angular/core';
import {Store} from "@ngrx/store";
import * as docSelectors from "../../state/selectors/docs.crud.selectors";
import {DocResponseModel} from "../../models/docs/docs.response.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DocCompareService {
  lastSave?: DocResponseModel;
  constructor(private store: Store) {
    this.store.select(docSelectors.selectCurrentDocLastSave)
      .pipe()
      .subscribe((lastSave) => {
        this.lastSave = lastSave;
      })
  }

  compare(doc: DocResponseModel): Observable<boolean> {
    if (this.lastSave && doc.id == this.lastSave.id){
      const comparisonResult =
        doc.title.localeCompare(this.lastSave.title) == 0 &&
        doc.content.localeCompare(this.lastSave.content) == 0;
      if (comparisonResult){
        return of(true);
      } else {
        return of(false);
      }
    } else {
      throw new Error('Last save is not defined or doc id is not equal to last save id.')
    }
  }
}
