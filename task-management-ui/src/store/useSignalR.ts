import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import useNotificationStore from "./useNotificationStore";
import useTaskCommentStore from "./useCommentStore";

const useSignalR = (taskId?: number) => {
  const addNotification = useNotificationStore(
    (state) => state.
  );
  const addTaskComment = useTaskCommentStore((state) => state.addTaskComment);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:7124/notificationHub")
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .catch((err) => console.error("SignalR Connection Error:", err));

    // Listen for notifications
    connection.on("ReceiveNotification", (notification) => {
      addNotification(notification);
    });

    // Listen for task comments
    connection.on("ReceiveTaskComment", (comment) => {
      addTaskComment(comment,);
    });

    // If taskId is provided, join the task group
    if (taskId) {
      connection.start().then(() => {
        connection.invoke("JoinTaskGroup", taskId);
      });

      return () => {
        connection.invoke("LeaveTaskGroup", taskId);
        connection.stop();
      };
    }

    return () => {
      connection.stop();
    };
  }, [taskId]);

  return null;
};

export default useSignalR;
