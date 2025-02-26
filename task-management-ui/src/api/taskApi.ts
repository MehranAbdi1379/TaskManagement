import axiosInstance from "./axiosInstance";

export interface Task {
  id: number;
  title: string;
  description: string;
  status: Status;
  createdAt: Date;
}

export interface TaskQueryParameters {
  pageNumber: number;
  pageSize: number;
  status?: Status;
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
