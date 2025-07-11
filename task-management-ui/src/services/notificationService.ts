import axiosInstance from "./axiosInstance";
import {Notification, NotificationQueryParameters} from "@/models/notification.ts";
import {PagedResult} from "@/models/globalModels.ts";


export const getNotificationsApi = async (
    params: NotificationQueryParameters
): Promise<PagedResult<Notification>> => {
    const response = await axiosInstance.get<PagedResult<Notification>>(
        "/notifications",
        {
            params,
        }
    );
    return response.data;
};

export const updateNotificationIsRead = async (id: number): Promise<void> => {
    const response = await axiosInstance.patch("/notifications/".concat(id.toString()));
    return response.data;
};
