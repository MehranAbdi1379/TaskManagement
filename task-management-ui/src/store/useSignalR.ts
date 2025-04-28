import { useEffect } from "react";
import useNotificationStore from "./useNotificationStore";
import { Notification } from "../api/notificationApi";
import { TaskComment } from "@/api/taskApi";
import useTaskCommentStore from "./useCommentStore";
import connection from "../api/signalRConnection";

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

  //const [isConnected, setIsConnected] = useState(false);

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
  }, [addNotification, addTaskComment, deleteTaskComment]);
};

export default useSignalR;
