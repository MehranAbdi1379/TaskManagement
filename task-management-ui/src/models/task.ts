import {SortOrder} from "@/models/globalModels.ts";

export interface Task {
    id: number;
    title: string;
    description: string;
    status: Status;
    createdAt: Date;
    isOwner: boolean;
}

export interface TaskComment {
    id: number;
    userFullName: string;
    taskId: number;
    text: string;
    createdAt: Date;
    userId: number;
    isOwner: boolean;
}

export interface TaskQueryParameters {
    pageNumber: number;
    pageSize: number;
    status?: Status;
    sortOrder: SortOrder;
}

export interface TaskCommentQueryParameters {
    pageNumber: number;
    pageSize: number;
    commentOwner: boolean;
    sortOrder: SortOrder;
}


export enum Status {
    Pending,
    InProgress,
    Completed,
    Cancelled,
}

