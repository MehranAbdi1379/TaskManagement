import {login, logout, register, User} from "@/services/authService";
import {create} from "zustand";

interface AuthState {
    isAuthenticated: boolean;
    login: (user: User) => Promise<void>;
    register: (
        email: string,
        password: string,
        firstName: string,
        lastName: string
    ) => Promise<void>;
    logout: () => void;
    setIsAuthenticated: (isAuthenticated: boolean) => void;
    userIsAuthenticated: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
    isAuthenticated: false,

    setIsAuthenticated: (isAuthenticated: boolean) => {
        set({isAuthenticated: isAuthenticated});
        localStorage.setItem("login state", isAuthenticated ? "true" : "false");
    },

    register: async (email, password, firstName, lastName) => {
        try {
            await register(email, password, firstName, lastName).then(() =>
                login({username: email, password})
            );
        } catch (error) {
            console.error("Register failed", error);
        }
    },
    userIsAuthenticated: async () => {
        const result = localStorage.getItem("login state");
        if (result == "true") {
            set({isAuthenticated: true});
        } else {
            set({isAuthenticated: false});
        }
    },
    login: async (user) => {
        try {
            await login(user);
            localStorage.setItem("login state", "true");
            set({isAuthenticated: true});
        } catch (error) {
            console.error("Login failed", error);
            throw new Error("Invalid credentials");
        }
    },

    logout: async () => {
        await logout()
            .then(() => {
                set({isAuthenticated: false});
                localStorage.setItem("login state", "false");
            })
            .catch((error) => console.error("Logout failed", error));
    },
}));
