import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import * as notificationsActions from "src/app/state/actions/notifications.actions";
import * as notificationsSelectors from "src/app/state/selectors/notifications.selectors";
import {snackBarConfig} from "../../../config/snackBarConfig";
import {Subject, take, takeUntil} from "rxjs";
import {MatSnackBar, MatSnackBarRef} from "@angular/material/snack-bar";

@Component({
  selector: 'notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.sass']
})
export class NotificationsComponent implements OnInit, OnDestroy{
  snackBarRef?: MatSnackBarRef<any>;
  subManager$ = new Subject<void>();
  constructor(private store: Store,
              private _snackBar: MatSnackBar){
    this.store.select(notificationsSelectors.selectNotificationsQueueLength)
      .pipe(takeUntil(this.subManager$))
      .subscribe(queueLength => {
        if (queueLength > 0 && !this.snackBarRef) {
          this.showNotification();
        }
      })
  }

  showNotification(){
    this.store.select(notificationsSelectors.selectOldestNotification)
      .pipe(take(1))
      .subscribe(notification => {
        this.snackBarRef = this._snackBar.open(notification.message, '', snackBarConfig);
        this.snackBarRef.afterDismissed()
          .pipe(takeUntil(this.subManager$))
          .subscribe(() => {
            this.store.dispatch(notificationsActions.dequeue_notification());
          });
      })
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.subManager$.next();
    this.subManager$.complete();
  }

}
