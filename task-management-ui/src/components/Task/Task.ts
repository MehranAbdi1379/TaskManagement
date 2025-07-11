import {CheckCircleIcon, CloseIcon, InfoIcon, WarningIcon,} from "@chakra-ui/icons";
import {Status} from "@/models/task.ts";

export const statusColors = {
    [Status.Pending]: "yellow",
    [Status.InProgress]: "blue",
    [Status.Completed]: "green",
    [Status.Cancelled]: "red",
};

export const statusIcons = {
    [Status.Pending]: WarningIcon,
    [Status.InProgress]: InfoIcon,
    [Status.Completed]: CheckCircleIcon,
    [Status.Cancelled]: CloseIcon,
};

export const getStatusLabel = (status: Status) => {
    switch (status) {
        case Status.Pending:
            return "Pending";
        case Status.InProgress:
            return "In Progress";
        case Status.Completed:
            return "Completed";
        case Status.Cancelled:
            return "Cancelled";
        default:
            return "Unknown";
    }
};
