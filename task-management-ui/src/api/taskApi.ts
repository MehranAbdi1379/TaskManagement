import axiosInstance from "./axiosInstance";

export const getTasks = async () => {
  const response = await axiosInstance.get("/task");
  return response.data;
};

export const createTask = async (task: any) => {
  const response = await axiosInstance.post("/task", task);
  return response.data;
};

export const updateTask = async (task: any) => {
  const response = await axiosInstance.put("/task", task);
  return response.data;
};

export const deleteTask = async (id: number) => {
  await axiosInstance.delete("/task/" + String(id));
};
