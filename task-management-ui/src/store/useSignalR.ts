import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import { LogLevel } from "@microsoft/signalr";
import useNotificationStore from "./useNotificationStore";
import { Notification } from "../api/notificationApi";
import { TaskComment } from "@/api/taskApi";
import useTaskCommentStore from "./useCommentStore";

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

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7124/hub") // Your backend Hub URL
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("✅ Connected to NotificationHub"))
      .catch((err) =>
        console.error("❌ Error connecting to NotificationHub:", err)
      );

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
      connection.stop(); // 👌 Clean up on unmount
    };
  }, [addNotification, addTaskComment, deleteTaskComment]);
};

export default useSignalR;
