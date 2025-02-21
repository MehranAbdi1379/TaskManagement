import React from "react";
import { Box, Button, Text } from "@chakra-ui/react";
import useTaskStore from "../store/useTaskStore";
import { Status } from "../api/taskApi";

const TaskList: React.FC = () => {
  const { tasks, removeTask } = useTaskStore(); // Get global state

  return (
    <Box>
      {tasks.length === 0 ? (
        <Text>No tasks available</Text>
      ) : (
        tasks.map((task) => (
          <Box key={task.id} p={4} borderWidth="1px" borderRadius="md" mb={2}>
            <Text fontWeight="bold">{task.title}</Text>
            <Text>Description: {task.description}</Text>
            <Text>Status: {Status[task.status]}</Text>
            <Button
              mt={2}
              colorScheme="red"
              onClick={() => removeTask(task.id)}
            >
              Delete
            </Button>
          </Box>
        ))
      )}
    </Box>
  );
};

export default TaskList;
