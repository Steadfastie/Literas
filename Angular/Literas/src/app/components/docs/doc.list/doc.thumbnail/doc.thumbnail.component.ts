import {AfterViewInit, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {DocThumbnail} from "../../../../models/docs/doc.thumbnail";
import { NavigationEnd, Router } from "@angular/router";
import {filter} from "rxjs";

@Component({
  selector: 'doc-thumbnail',
  templateUrl: './doc.thumbnail.component.html',
  styleUrls: ['./doc.thumbnail.component.sass']
})
export class DocThumbnailComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() thumbnail!: DocThumbnail;
  isActive: boolean = false;
  constructor(private store: Store,
              private router: Router) {

  }


  ngOnInit(): void {
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(e => {
        const url = (e as NavigationEnd).url.split('/')[2];
        this.isActive = this.thumbnail.id.toString() == url;
      })
  }


  ngAfterViewInit(): void {

  }


  ngOnDestroy(): void {
  }
}
