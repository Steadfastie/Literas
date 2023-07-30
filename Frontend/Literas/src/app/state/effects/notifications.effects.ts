import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {Router} from "@angular/router";
import * as notificationsActions from "../actions/notifications.actions";
import {tap} from "rxjs";
import {Store} from "@ngrx/store";

@Injectable({
  providedIn: 'root'
})
export class NotificationsEffects {

  constructor(private actions$: Actions,
              private router: Router,
              private store: Store) { }


  errorModelToNotification$ = createEffect(() => this.actions$.pipe(
      ofType(notificationsActions.enqueue_error_notification),
      tap(error => {
         this.store.dispatch(notificationsActions.enqueue_notification({
            message: error.message,
            invocationDateTime: new Date()
         }))
      })),
    {dispatch: false})
}
