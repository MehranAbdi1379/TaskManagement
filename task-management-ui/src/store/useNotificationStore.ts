import {NotificationQueryParameters} from "@/models/notification.ts";
import {create} from "zustand";
import {getUserId} from "@/services/authService.ts";
import {getNotificationsApi, updateNotificationIsRead} from "@/services/notificationService.ts";
import {Notification} from "@/models/notification"


interface NotificationStore {
    notifications: Notification[];
    addNotification: (notification: Notification) => Promise<void>;
    loading: boolean;
    getNotifications: (
        parameters: NotificationQueryParameters
    ) => Promise<void>;
    updateNotificationIsRead: (id: number) => Promise<void>;
    notificationActiveOrHistory: "active" | "history";
    totalPages: number;
}

// Create Zustand store
const useNotificationStore = create<NotificationStore>((set) => ({
    notifications: [],
    totalPages: 0,
    notificationActiveOrHistory: "history",
    addNotification: async (notification) => {
        const userId = await getUserId();
        if (userId == notification.userId) {
            set((state) => ({
                notifications: [...state.notifications, notification],
            }));
        }
    },
    loading: true,

    getNotifications: async (parameters: NotificationQueryParameters) => {
        try {
            const notifications = await getNotificationsApi(parameters);
            set({notifications: notifications.items});
            set({notificationActiveOrHistory: parameters.isHistory ? "history" : "active"});
            set({totalPages: notifications.totalPages});
        } catch (error) {
            console.error("Error fetching notifications:", error);
        } finally {
            set({loading: false});
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
