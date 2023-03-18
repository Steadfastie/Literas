import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import * as quillSelectionsSelectors from "src/app/state/selectors/quill.selection.selectors";
import * as quillSelectionActions from 'src/app/state/actions/quill.selection.actions';
import {Subject, takeUntil} from "rxjs";
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'quill-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.sass']
})
export class LinkComponent implements OnInit, OnDestroy{
  hasUrl: boolean = false;
  url?: string;
  inputOpened: boolean = false;
  subManager$: Subject<any> = new Subject();

  constructor(private store: Store,
              private fb: FormBuilder) {
    this.store.select(quillSelectionsSelectors.selectCurrentSelectionFormats)
      .pipe(takeUntil(this.subManager$))
      .subscribe(selections => {
        this.hasUrl = selections['link'].length > 3;
        if (this.hasUrl) this.url = selections['link'];
      });

    this.store.select(quillSelectionsSelectors.selectLinkInputOpenState)
      .pipe(takeUntil(this.subManager$))
      .subscribe(status => {
        this.inputOpened = status;
      });
  }
  urlForm = this.fb.group({
    url: ['', Validators.minLength(3)],
  })

  switchInput(){
    this.store.dispatch(quillSelectionActions.quill_switchLinkInput());
  }

  submit(){
    if(this.urlForm.invalid) return;
    if(this.urlForm.value.url?.length! < 3) return;
    this.store.dispatch(
      quillSelectionActions.quill_formatChange(
      {format: 'link', value: this.urlForm.value.url}
      )
    );
  }
  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.subManager$.next('destroyed');
  }

}
