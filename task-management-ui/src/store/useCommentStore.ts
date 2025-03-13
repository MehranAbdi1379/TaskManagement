import {
  createTaskComment,
  deleteTaskComment,
  getTaskComments,
  TaskComment,
  TaskCommentQueryParameters,
} from "../api/taskApi";
import { create } from "zustand";

interface TaskCommentStore {
  comments: TaskComment[];
  loading: boolean;
  getTaskComments: (
    id: number,
    parameters: TaskCommentQueryParameters
  ) => Promise<void>;
  addTaskComment: (text: string, taskId: number) => Promise<void>;
  removeTaskComment: (taskCommentId: number) => Promise<void>;
}

// Create Zustand store
const useTaskCommentStore = create<TaskCommentStore>((set) => ({
  comments: [],
  loading: true,

  getTaskComments: async (
    id: number,
    parameters: TaskCommentQueryParameters
  ) => {
    try {
      const taskComments = await getTaskComments(id, parameters);
      set({ comments: taskComments.items });
    } catch (error) {
      console.error("Error fetching taskComments:", error);
    } finally {
      set({ loading: false });
    }
  },

  addTaskComment: async (text: string, taskId: number) => {
    try {
      const newTaskComment = await createTaskComment(text, taskId);
      set((state) => ({ comments: [...state.comments, newTaskComment] }));
    } catch (error) {
      console.error("Error adding taskComment:", error);
    }
  },

  removeTaskComment: async (taskCommentId) => {
    try {
      await deleteTaskComment(taskCommentId);
      set((state) => ({
        comments: state.comments.filter((t) => t.id !== taskCommentId),
      }));
    } catch (error) {
      console.error("Error deleting taskComment:", error);
    }
  },
}));

export default useTaskCommentStore;
