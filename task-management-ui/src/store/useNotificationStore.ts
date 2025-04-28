import { create } from "zustand";
import {
  getActiveNotifications,
  getNotificationHistory,
  NotificationQueryParameters,
  Notification,
  updateNotificationIsRead,
} from "../api/notificationApi";

interface NotificationStore {
  notifications: Notification[];
  addNotification: (notification: Notification) => void;
  loading: boolean;
  getActiveNotifications: (
    parameters: NotificationQueryParameters
  ) => Promise<void>;
  getNotificationHistory: (
    parameters: NotificationQueryParameters
  ) => Promise<void>;
  updateNotificationIsRead: (id: number) => Promise<void>;
}

// Create Zustand store
const useNotificationStore = create<NotificationStore>((set) => ({
  notifications: [],
  addNotification: (notification) =>
    set((state) => ({
      notifications: [...state.notifications, notification],
    })),
  loading: true,

  getActiveNotifications: async (parameters: NotificationQueryParameters) => {
    try {
      const notifications = await getActiveNotifications(parameters);
      set({ notifications: notifications.items });
    } catch (error) {
      console.error("Error fetching notifications:", error);
    } finally {
      set({ loading: false });
    }
  },

  getNotificationHistory: async (parameters: NotificationQueryParameters) => {
    try {
      const notifications = await getNotificationHistory(parameters);
      set({ notifications: notifications.items });
    } catch (error) {
      console.error("Error fetching tasks:", error);
    } finally {
      set({ loading: false });
    }
  },

  updateNotificationIsRead: async (id) => {
    try {
      await updateNotificationIsRead(id);
      set((state) => ({
        notifications: state.notifications.filter((t) => !t.isRead),
      }));
    } catch (error) {
      console.error("Error updating notification status:", error);
    }
  },
}));

export default useNotificationStore;
