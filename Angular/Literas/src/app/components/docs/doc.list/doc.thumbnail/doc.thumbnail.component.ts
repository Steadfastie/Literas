import {AfterViewInit, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {DocThumbnail} from "../../../../models/docs/doc.thumbnail";
import * as docSelectors from 'src/app/state/selectors/docs.crud.selectors';
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'doc-thumbnail',
  templateUrl: './doc.thumbnail.component.html',
  styleUrls: ['./doc.thumbnail.component.sass']
})
export class DocThumbnailComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() thumbnail!: DocThumbnail;
  isActive: boolean = false;
  subManager$: Subject<any> = new Subject();
  constructor(private store: Store) {

  }
  ngOnInit(): void {
    this.store.select(docSelectors.selectUrlIdState)
      .pipe(takeUntil(this.subManager$))
      .subscribe(id => {
        if (id){
          this.isActive = this.thumbnail.id === id;
        }
      })
  }
  ngAfterViewInit(): void {
  }

  ngOnDestroy(): void {
    this.subManager$.next('destroyed')
  }
}
