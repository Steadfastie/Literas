import {createAction, props} from "@ngrx/store";
import {Notification} from "src/app/models/system/notification";
import {ErrorModel} from "../../models/system/error.model";

export enum NotificationsActions {
  'PushNotification' = '[Notifications] Push Notification',
  'PushErrorNotification' = '[Notifications] Push Error Notification',
  'PullNotification' = '[Notifications] Pull Notification',
}

export const enqueue_notification = createAction(
  NotificationsActions.PushNotification,
  props<Notification>()
)

export const enqueue_error_notification = createAction(
  NotificationsActions.PushErrorNotification,
  props<ErrorModel>()
)

export const dequeue_notification = createAction(
  NotificationsActions.PullNotification,
)
