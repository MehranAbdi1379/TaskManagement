import { create } from "zustand";
import {
  getTasks,
  createTask,
  updateTask,
  deleteTask,
  Task,
  TaskQueryParameters,
} from "../api/taskApi";

interface TaskStore {
  tasks: Task[];
  loading: boolean;
  getTasks: (parameters: TaskQueryParameters) => Promise<void>;
  addTask: (task: Omit<Task, "id">) => Promise<void>;
  updateTask: (task: Task) => Promise<void>;
  removeTask: (taskId: number) => Promise<void>;
}

// Create Zustand store
const useTaskStore = create<TaskStore>((set) => ({
  tasks: [],
  loading: true,

  getTasks: async (parameters: TaskQueryParameters) => {
    try {
      const tasks = await getTasks(parameters);
      set({ tasks: tasks.items });
    } catch (error) {
      console.error("Error fetching tasks:", error);
    } finally {
      set({ loading: false });
    }
  },

  addTask: async (task) => {
    try {
      const newTask = await createTask(task);
      set((state) => ({ tasks: [...state.tasks, newTask] }));
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

  removeTask: async (taskId) => {
    try {
      await deleteTask(taskId);
      set((state) => ({ tasks: state.tasks.filter((t) => t.id !== taskId) }));
    } catch (error) {
      console.error("Error deleting task:", error);
    }
  },
}));

export default useTaskStore;
