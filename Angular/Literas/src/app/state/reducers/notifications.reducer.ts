import {NotificationsState} from "../models/notifications.state";
import {createReducer, on} from "@ngrx/store";
import * as notificationsActions from "../actions/notifications.actions";

export const notificationsInitialState: NotificationsState = {
  notificationsBuffer: [],
}

export const notificationsReducer = createReducer(
  notificationsInitialState,

  on(notificationsActions.enqueue_notification,
    (state, notification) => ({...state, notificationsBuffer: [...state.notificationsBuffer, notification]})
  ),
  on(notificationsActions.dequeue_notification,
    (state) => {
    if (state.notificationsBuffer.length > 0) {
      return ({...state, notificationsBuffer: state.notificationsBuffer.slice(1)})
    }
    return state;
    })
);
