import {create} from "zustand";
import {TaskComment, TaskCommentQueryParameters} from "@/models/task.ts";
import {getUserId} from "@/services/authService.ts";
import {createTaskComment, deleteTaskComment, getTaskCommentsApi} from "@/services/taskService.ts";

interface TaskCommentStore {
    comments: TaskComment[];
    loading: boolean;
    getTaskComments: (
        id: number,
        parameters: TaskCommentQueryParameters
    ) => Promise<void>;
    addTaskComment: (text: string, taskId: number) => Promise<void>;
    removeTaskComment: (taskCommentId: number) => Promise<void>;
    addTaskCommentFromStateManagement: (
        taskComment: TaskComment
    ) => Promise<void>;
    removeTaskCommentFromStateManagement: (id: number) => void;
    totalPages: number;
}

// Create Zustand store
const useTaskCommentStore = create<TaskCommentStore>((set) => ({
    comments: [],
    totalPages: 0,
    loading: true,
    removeTaskCommentFromStateManagement: (id) =>
        set((state) => ({comments: state.comments.filter((t) => t.id !== id)})),
    addTaskCommentFromStateManagement: async (taskComment) => {
        const userId = await getUserId();
        if (userId != taskComment.userId) {
            set((state) => ({
                comments: [...state.comments, taskComment],
            }));
        }
    },

    getTaskComments: async (
        id: number,
        parameters: TaskCommentQueryParameters
    ) => {
        try {
            const taskComments = await getTaskCommentsApi(id, parameters);
            set({comments: taskComments.items});
            set({totalPages: taskComments.totalPages});
        } catch (error) {
            console.error("Error fetching taskComments:", error);
        } finally {
            set({loading: false});
        }
    },

    addTaskComment: async (text: string, taskId: number) => {
        try {
            const taskComment = await createTaskComment(text, taskId);
            set((state) => ({
                comments: [...state.comments, taskComment],
            }));
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
