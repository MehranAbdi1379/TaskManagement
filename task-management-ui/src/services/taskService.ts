import {Status, Task, TaskComment, TaskCommentQueryParameters, TaskQueryParameters} from "@/models/task";
import axiosInstance from "./axiosInstance";
import {PagedResult} from "@/models/globalModels.ts";


export const getTasks = async (
    params: TaskQueryParameters
): Promise<PagedResult<Task>> => {
    const response = await axiosInstance.get<PagedResult<Task>>("/tasks", {
        params,
    });
    return response.data;
};

export const getTaskById = async (id: number): Promise<Task> => {
    const response = await axiosInstance.get("/tasks/".concat(String(id)));
    return response.data;
};

export const createTask = async (task: Omit<Task, "id">): Promise<Task> => {
    const response = await axiosInstance.post("/tasks", task);
    return response.data;
};

export const updateTask = async (
    task: Omit<Task, "createdAt">
): Promise<Task> => {
    const response = await axiosInstance.put("/tasks/".concat(task.id.toString()), task);
    return response.data;
};

export const updateTaskStatus = async (
    id: number,
    status: Status
): Promise<Task> => {
    const response = await axiosInstance.patch("/tasks/".concat(id.toString()), {status});
    return response.data;
};

export const deleteTask = async (id: number): Promise<void> => {
    await axiosInstance.delete("/tasks/".concat(String(id)));
};

export const AssignTaskToUserRequest = async (
    assigneeEmail: string,
    taskId: number
): Promise<void> => {
    const response = await axiosInstance.post("/tasks/".concat(taskId.toString()).concat("/assignees"), {
        assigneeEmail,
    });
    return response.data;
};

export const AssignTaskToUserRespond = async (
    requestNotificationId: number,
    accept: boolean
): Promise<void> => {
    const response = await axiosInstance.post("/tasks/".concat("0/assignees/").concat(requestNotificationId.toString()), {
        requestNotificationId,
        accept,
    });
    return response.data;
};

export const getTaskCommentsApi = async (
    id: number,
    params: TaskCommentQueryParameters
): Promise<PagedResult<TaskComment>> => {
    const response = await axiosInstance.get<PagedResult<TaskComment>>(
        "/tasks/".concat(String(id)).concat("/comments"),
        {
            params,
        }
    );
    return response.data;
};

export const createTaskComment = async (
    text: string,
    id: number
): Promise<TaskComment> => {
    const response = await axiosInstance.post<TaskComment>(
        "/tasks/".concat(String(id).concat("/comments")),
        {
            text,
        }
    );
    return response.data;
};

export const deleteTaskComment = async (id: number): Promise<void> => {
    await axiosInstance.delete(
        "/tasks/".concat("0").concat("/comments/").concat(String(id))
    );
};

export const getAssignedUsers = async (
    id: number
): Promise<
    { userId: number; firstName: string; lastName: string; email: string }[]
> => {
    const response = await axiosInstance.get<
        { firstName: string; lastName: string; email: string; userId: number }[]
    >("/tasks/".concat(String(id)).concat("/assignees"));
    return response.data;
};

export const unassignTaskUser = async (
    taskId: number,
    userId: number
): Promise<void> => {
    await axiosInstance.delete(
        "/tasks/"
            .concat(String(taskId))
            .concat("/assignees/")
            .concat(String(userId))
    );
};
