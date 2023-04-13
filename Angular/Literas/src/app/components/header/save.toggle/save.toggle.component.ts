import {Component, OnDestroy} from '@angular/core';
import * as docSelectors from "../../../state/selectors/docs.crud.selectors";
import {Store} from "@ngrx/store";
import {Subject, takeUntil} from "rxjs";
import {SaveToggleService} from "../../../services/header/save.toggle.service";

@Component({
  selector: 'save-toggle',
  templateUrl: './save.toggle.component.html',
  styleUrls: ['./save.toggle.component.sass']
})
export class SaveToggleComponent implements OnDestroy{
  isSaving: boolean = false
  subManager$ = new Subject<void>();
  constructor(private store: Store,
              private saveToggleService: SaveToggleService) {
    this.store.select(docSelectors.selectSavingState)
      .pipe(takeUntil(this.subManager$))
      .subscribe((saving) => {
      this.isSaving = saving
    })
  }
  saveCurrentDoc() {
    this.saveToggleService.activateManual();
  }

  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }
}
