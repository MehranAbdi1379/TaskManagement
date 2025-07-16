import connection from "@/services/signalRService.ts";
import useNotificationStore from "@/store/useNotificationStore.ts";
import useTaskCommentStore from "@/store/useCommentStore.ts";
import {useEffect} from "react";
import {TaskComment} from "@/models/task.ts";
import {Notification} from "@/models/notification.ts";
import {useAuthStore} from "@/store/useAuthStore.ts";


const useSignalR = () => {
    const addNotification = useNotificationStore(
        (state) => state.addNotification
    );
    const addTaskComment = useTaskCommentStore(
        (state) => state.addTaskCommentFromStateManagement
    );
    const deleteTaskComment = useTaskCommentStore(
        (state) => state.removeTaskCommentFromStateManagement
    );
    const isAuthenticated = useAuthStore(
        (state) => state.isAuthenticated
    );

    useEffect(() => {
        if (connection.state === "Disconnected") {
            connection
                .start()
                .then(() => console.log("✅ Connected to NotificationHub"))
                .catch((err) =>
                    console.error("❌ Error connecting to NotificationHub:", err)
                );
        }

        connection.on("ReceiveNotification", (notification: Notification) => {
            console.log("📩 Received Notification:", notification);
            addNotification(notification); // ✅ Update the Zustand store
        });

        connection.on("ReceiveTaskComment", (taskComment: TaskComment) => {
            console.log("📩 Received Task Comment:", taskComment);
            addTaskComment(taskComment); // ✅ Update the Zustand store
        });

        connection.on("DeleteTaskComment", (id: number) => {
            console.log("Deleted Task Comment Id:", id);
            deleteTaskComment(id); // ✅ Update the Zustand store
        });

        return () => {
            connection.off("ReceiveNotification");
            connection.off("ReceiveTaskComment");
            connection.off("DeleteTaskComment");
            if (connection.state === "Connected") {
                connection
                    .stop()
                    .then(() => console.log("❌ Disconnected from NotificationHub"));
            }
        };
    }, [addNotification, addTaskComment, deleteTaskComment, isAuthenticated]);
};

export default useSignalR;
