import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {IDocThumbnail} from "../../../../models/docs/doc.thumbnail";

@Component({
  selector: 'doc-thumbnail',
  templateUrl: './doc.thumbnail.component.html',
  styleUrls: ['./doc.thumbnail.component.sass']
})
export class DocThumbnailComponent implements OnInit, OnDestroy {
  @Input() thumbnail!: IDocThumbnail;
  constructor(private store: Store) {
  }
  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

}
