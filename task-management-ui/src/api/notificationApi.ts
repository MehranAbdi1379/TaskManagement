import axiosInstance from "./axiosInstance";
import { PagedResult } from "./taskApi";

export interface Notification {
  id: number;
  userId: number;
  title: string;
  content: string;
  type: "General" | "TaskAssignmentRequest";
  isRead: boolean;
}

export interface NotificationQueryParameters {
  pageNumber: number;
  pageSize: number;
  sortOrder: "asc" | "desc";
}

export const getActiveNotifications = async (
  params: NotificationQueryParameters
): Promise<PagedResult<Notification>> => {
  const response = await axiosInstance.get<PagedResult<Notification>>(
    "/notification/active",
    {
      params,
    }
  );
  return response.data;
};

export const getNotificationHistory = async (
  params: NotificationQueryParameters
): Promise<PagedResult<Notification>> => {
  const response = await axiosInstance.get<PagedResult<Notification>>(
    "/notification/history",
    {
      params,
    }
  );
  return response.data;
};

export const updateNotificationIsRead = async (id: number): Promise<void> => {
  const response = await axiosInstance.patch("/notification", id);
  return response.data;
};
