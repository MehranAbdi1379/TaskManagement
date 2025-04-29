import { create } from "zustand";
import {
  getActiveNotifications,
  getNotificationHistory,
  NotificationQueryParameters,
  Notification,
  updateNotificationIsRead,
} from "../api/notificationApi";
import { getUserId } from "../api/authApi";

interface NotificationStore {
  notifications: Notification[];
  addNotification: (notification: Notification) => Promise<void>;
  loading: boolean;
  getActiveNotifications: (
    parameters: NotificationQueryParameters
  ) => Promise<void>;
  getNotificationHistory: (
    parameters: NotificationQueryParameters
  ) => Promise<void>;
  updateNotificationIsRead: (id: number) => Promise<void>;
  notificationActiveOrHitory: "active" | "history";
}

// Create Zustand store
const useNotificationStore = create<NotificationStore>((set) => ({
  notifications: [],
  notificationActiveOrHitory: "history",
  addNotification: async (notification) => {
    const userId = await getUserId();
    if (userId == notification.userId) {
      set((state) => ({
        notifications: [...state.notifications, notification],
      }));
    }
  },
  loading: true,

  getActiveNotifications: async (parameters: NotificationQueryParameters) => {
    try {
      const notifications = await getActiveNotifications(parameters);
      set({ notifications: notifications.items });
      set({ notificationActiveOrHitory: "active" });
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
      set({ notificationActiveOrHitory: "history" });
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
