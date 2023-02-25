import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'doc-list',
  templateUrl: './doc.list.component.html',
  styleUrls: ['./doc.list.component.sass']
})
export class DocListComponent implements OnInit, OnDestroy {

  constructor(){}
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

}
