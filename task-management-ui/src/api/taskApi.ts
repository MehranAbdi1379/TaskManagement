import axiosInstance from "./axiosInstance";

export interface Task {
  id: number;
  title: string;
  description: string;
  status: Status;
}

export enum Status {
  Pending,
  InProgress,
  Completed,
  Cancelled,
}

export const getTasks = async (): Promise<Task[]> => {
  const response = await axiosInstance.get<Task[]>("/task");
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

export const updateTask = async (task: Task): Promise<Task> => {
  const response = await axiosInstance.put("/task", task);
  return response.data;
};

export const deleteTask = async (id: number): Promise<void> => {
  await axiosInstance.delete("/task/".concat(String(id)));
};
