import { Notification } from 'src/app/models/system/notification';

export interface NotificationsState {
  notificationsBuffer: Notification[],
}
