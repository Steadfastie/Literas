import { Component, OnDestroy, OnInit } from '@angular/core';
import {Store} from "@ngrx/store";
import {IDocThumbnail} from "../../../models/docs/doc.thumbnail";
import * as docActions from "../../../state/actions/docs.crud.actions";

@Component({
  selector: 'doc-list',
  templateUrl: './doc.list.component.html',
  styleUrls: ['./doc.list.component.sass']
})
export class DocListComponent implements OnInit, OnDestroy {
  docs: IDocThumbnail[] = [];
  constructor(private store: Store){
    this.store.dispatch(docActions.doc_thumbnails_fetch());
  }
  ngOnInit(): void {

  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

}
