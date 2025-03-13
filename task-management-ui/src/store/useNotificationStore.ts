import { create } from "zustand";
import {
  getActiveNotifications,
  getNotificationHistory,
  NotificationQueryParameters,
  Notification,
  updateNotificationIsRead,
} from "../api/notificationApi";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

interface NotificationStore {
  notifications: Notification[];
  loading: boolean;
  connection: HubConnection | null;
  connect: () => void;
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
  loading: true,

  connection: null,

  connect: () => {
    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7124/notificationHub") // Update the URL if necessary
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("Connected to NotificationHub"))
      .catch((err) =>
        console.error("Error connecting to NotificationHub:", err)
      );

    connection.on("ReceiveNotification", (notification: Notification) => {
      set((state) => ({
        notifications: [notification, ...state.notifications],
      }));
    });

    set({ connection });
  },

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
      const tasks = await getNotificationHistory(parameters);
      set({ notifications: tasks.items });
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
