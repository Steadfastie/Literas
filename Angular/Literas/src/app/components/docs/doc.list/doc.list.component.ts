import { Component, OnDestroy, OnInit } from '@angular/core';
import {Store} from "@ngrx/store";
import {IDocThumbnail} from "../../../models/docs/doc.thumbnail";
import * as docActions from "../../../state/actions/docs.crud.actions";
import * as selectors from "../../../state/selectors/docs.crud.selectors";
import {Observable} from "rxjs";

@Component({
  selector: 'doc-list',
  templateUrl: './doc.list.component.html',
  styleUrls: ['./doc.list.component.sass']
})
export class DocListComponent implements OnInit, OnDestroy {
  docs: IDocThumbnail[] = [];
  constructor(private store: Store){
    this.store.select(selectors.selectDocThumbnails)
      .pipe()
      .subscribe(thumbnails => this.docs = thumbnails);
  }
  ngOnInit(): void {
    this.store.dispatch(docActions.doc_thumbnails_fetch());
  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

}
