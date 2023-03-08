import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";

@Component({
  selector: 'doc-new',
  templateUrl: './doc.new.component.html',
  styleUrls: ['./doc.new.component.sass']
})
export class DocNewComponent implements OnInit, OnDestroy {
  constructor(private store: Store) {
  }
  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

}
