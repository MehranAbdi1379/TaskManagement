import React, { useEffect } from "react";
import { Box, Center, Spinner } from "@chakra-ui/react";
import useTaskStore from "./store/useTaskStore";
import TaskList from "./components/TaskList";

const App: React.FC = () => {
  const fetchTasks = useTaskStore((state) => state.getTasks);
  const loading = useTaskStore((state) => state.loading);

  useEffect(() => {
    fetchTasks(); // Fetch tasks from API when app loads
  }, [fetchTasks]);

  if (loading) {
    return (
      <Center height="100vh">
        <Spinner size="xl" color="blue.500" thickness="4px" speed="0.65s" />
      </Center>
    );
  }

  return (
    <Box p={4}>
      <TaskList />
    </Box>
  );
};

export default App;
