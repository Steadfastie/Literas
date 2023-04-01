import { Component, OnDestroy, OnInit } from '@angular/core';
import {Store} from "@ngrx/store";
import {DocThumbnail} from "../../../models/docs/doc.thumbnail";
import * as docActions from "../../../state/actions/docs.crud.actions";
import * as selectors from "../../../state/selectors/docs.crud.selectors";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'doc-list',
  templateUrl: './doc.list.component.html',
  styleUrls: ['./doc.list.component.sass']
})
export class DocListComponent implements OnInit, OnDestroy {
  docs: DocThumbnail[] = [];
  subManager$: Subject<any> = new Subject();
  constructor(private store: Store){
    this.store.select(selectors.selectDocThumbnails)
      .pipe(takeUntil(this.subManager$))
      .subscribe(thumbnails => this.docs = thumbnails);
  }
  ngOnInit(): void {
    this.store.dispatch(docActions.doc_thumbnails_fetch());
  }
  ngOnDestroy(): void {
    this.subManager$.next('unsubscribed');
  }

}
