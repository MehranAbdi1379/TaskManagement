import {Status, Task, TaskQueryParameters} from "@/models/task.ts";
import {create} from "zustand";
import {createTask, getAssignedUsers, getTaskById, getTasks, unassignTaskUser} from "@/services/taskService.ts";


interface TaskStore {
    tasks: Task[];
    loading: boolean;
    totalPages: number;
    assignedUsers: {
        userId: number;
        firstName: string;
        lastName: string;
        email: string;
    }[];
    getAssignedUsers: (taskId: number) => Promise<void>;
    getTasks: (parameters: TaskQueryParameters) => Promise<void>;
    addTask: (task: Omit<Task, "id">) => Promise<void>;
    updateTask: (task: Omit<Task, "createdAt">) => Promise<void>;
    updateTaskStatus: (id: number, status: Status) => Promise<void>;
    removeTask: (taskId: number) => Promise<void>;
    getTaskById: (id: number) => Promise<void>;
    removeTaskUser: (taskId: number, userId: number) => Promise<void>;
}

// Create Zustand store
const useTaskStore = create<TaskStore>((set) => ({
    tasks: [],
    totalPages: 0,
    loading: true,
    assignedUsers: [],
    removeTaskUser: async (taskId, userId) => {
        try {
            await unassignTaskUser(taskId, userId);
            set((state) => ({
                assignedUsers: state.assignedUsers.filter(
                    (user) => user.userId !== userId
                ),
            }));
        } catch (error) {
            console.error("Error removing task user:", error);
        }
    },
    getAssignedUsers: async (taskId: number) => {
        const response = await getAssignedUsers(taskId);
        set({assignedUsers: response});
    },

    getTasks: async (parameters: TaskQueryParameters) => {
        try {
            const tasks = await getTasks(parameters);
            set({tasks: tasks.items});
            set({totalPages: tasks.totalPages});
        } catch (error) {
            console.error("Error fetching tasks:", error);
        } finally {
            set({loading: false});
        }
    },

    getTaskById: async (id: number) => {
        set({loading: true});
        try {
            const task = await getTaskById(id);
            set((state) => ({
                tasks: [...state.tasks.filter((t) => t.id !== id), task], // Replace the existing task
            }));
        } catch (error) {
            console.error("Error fetching task:", error);
        } finally {
            set({loading: false});
        }
    },

    addTask: async (task) => {
        try {
            const newTask = await createTask(task);
            set((state) => ({tasks: [...state.tasks, newTask]}));
        } catch (error) {
            console.error("Error adding task:", error);
        }
    },

    updateTask: async (task) => {
        try {
            const updatedTask = await updateTask(task);
            set((state) => ({
                tasks: state.tasks.map((t) =>
                    t.id === updatedTask.id ? updatedTask : t
                ),
            }));
        } catch (error) {
            console.error("Error updating task:", error);
        }
    },

    updateTaskStatus: async (id, status) => {
        try {
            const updatedTask = await updateTaskStatus(id, status);
            set((state) => ({
                tasks: state.tasks.map((t) =>
                    t.id === updatedTask.id ? updatedTask : t
                ),
            }));
        } catch (error) {
            console.error("Error updating task status:", error);
        }
    },

    removeTask: async (taskId) => {
        try {
            await deleteTask(taskId);
            set((state) => ({tasks: state.tasks.filter((t) => t.id !== taskId)}));
        } catch (error) {
            console.error("Error deleting task:", error);
        }
    },
}));

export default useTaskStore;
