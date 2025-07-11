import {SortOrder} from "@/models/globalModels.ts";

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
    sortOrder: SortOrder;
    isHistory: boolean;
}
