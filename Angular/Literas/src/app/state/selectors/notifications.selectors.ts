import {createFeatureSelector, createSelector} from "@ngrx/store";
import {NotificationsState} from "../models/notifications.state";

export const selectNotificationsState = createFeatureSelector<NotificationsState>('notifications');

export const selectNotificationsQueueLength = createSelector(
  selectNotificationsState,
  (state) => state.notificationsBuffer.length
)

export const selectOldestNotification = createSelector(
  selectNotificationsState,
  (state) => state.notificationsBuffer[0]
)
