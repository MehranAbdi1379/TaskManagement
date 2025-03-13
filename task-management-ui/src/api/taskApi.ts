import axiosInstance from "./axiosInstance";

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
  sortOrder: "asc" | "desc";
}

export interface TaskCommentQueryParameters {
  pageNumber: number;
  pageSize: number;
  commentOwner: boolean;
  sortOrder: "asc" | "desc";
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
}

export enum Status {
  Pending,
  InProgress,
  Completed,
  Cancelled,
}

export const getTasks = async (
  params: TaskQueryParameters
): Promise<PagedResult<Task>> => {
  const response = await axiosInstance.get<PagedResult<Task>>("/task", {
    params,
  });
  return response.data;
};

export const getTaskById = async (id: number): Promise<Task> => {
  const response = await axiosInstance.get("/task/".concat(String(id)));
  return response.data;
};

export const createTask = async (task: Omit<Task, "id">): Promise<Task> => {
  const response = await axiosInstance.post("/task", task);
  return response.data;
};

export const updateTask = async (
  task: Omit<Task, "createdAt">
): Promise<Task> => {
  const response = await axiosInstance.put("/task", task);
  return response.data;
};

export const updateTaskStatus = async (
  id: number,
  status: Status
): Promise<Task> => {
  const response = await axiosInstance.patch("/task", { id, status });
  return response.data;
};

export const deleteTask = async (id: number): Promise<void> => {
  await axiosInstance.delete("/task/".concat(String(id)));
};

export const AssignTaskToUserRequest = async (
  assigneeEmail: string,
  taskId: number
): Promise<void> => {
  const response = await axiosInstance.post("/task/assign", {
    assigneeEmail,
    taskId,
  });
  return response.data;
};

export const AssignTaskToUserRespond = async (
  requestNotificationId: number,
  accept: boolean
): Promise<void> => {
  const response = await axiosInstance.post("/task/respond", {
    requestNotificationId,
    accept,
  });
  return response.data;
};

export const getTaskComments = async (
  id: number,
  params: TaskCommentQueryParameters
): Promise<PagedResult<TaskComment>> => {
  const response = await axiosInstance.get<PagedResult<TaskComment>>(
    "/task/".concat(String(id)).concat("/comment"),
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
    "/task/".concat(String(id).concat("/comment")),
    {
      text,
    }
  );
  return response.data;
};

export const deleteTaskComment = async (id: number): Promise<void> => {
  await axiosInstance.delete(
    "/task/".concat("0").concat("/comment/").concat(String(id))
  );
};
