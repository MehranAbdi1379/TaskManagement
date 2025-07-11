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
    const response = await axiosInstance.post("/users", {
        userName: email,
        firstName,
        lastName,
        password,
    });
    return response.data;
};

export const getUserId = async (): Promise<number> => {
    return (await axiosInstance.get("/me")).data.userId;
};

export const login = async (user: User): Promise<void> => {
    const response = await axiosInstance.post("/sessions", user);
    return response.data;
};

export const logout = async (): Promise<void> => {
    const response = await axiosInstance.delete("/sessions");
    return response.data;
};
