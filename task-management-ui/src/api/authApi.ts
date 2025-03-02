import axiosInstance from "./axiosInstance";

export interface User {
  username: string;
  password: string;
}

export const register = async (
  email: string,
  password: string,
  firstName: string,
  lastName: string
): Promise<void> => {
  const response = await axiosInstance.post("/Auth/register", {
    userName: email,
    firstName,
    lastName,
    password,
  });
  return response.data;
};

export const login = async (user: User): Promise<void> => {
  const response = await axiosInstance.post("/Auth/login", user);
  console.log(response);
  return response.data;
};

export const logout = async (): Promise<void> => {
  const response = await axiosInstance.post("Auth/logout");
  return response.data;
};
