import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Text,
  SimpleGrid,
  Badge,
  VStack,
  HStack,
  Icon,
  Center,
  Spinner,
  Flex,
  Select,
} from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";
import { InfoIcon, DeleteIcon } from "@chakra-ui/icons";
import useTaskStore from "../../store/useTaskStore";
import { getStatusLabel, statusColors, statusIcons } from "./Task";
import { Status, getTasks as getTasksApi } from "../../api/taskApi";

// Map enum values to corresponding colors and icons

const TaskList: React.FC = () => {
  const { tasks, removeTask, getTasks } = useTaskStore();
  const loading = useTaskStore((state) => state.loading);

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [statusFilter, setStatusFilter] = useState<Status | "">("");
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");
  const [totalPages, setTotalPages] = useState(1);

  const fetchTasks = async () => {
    await getTasks({
      pageNumber,
      pageSize,
      status: statusFilter || undefined,
      sortOrder,
    });
    const response = await getTasksApi({
      pageNumber,
      pageSize,
      status: statusFilter || undefined,
      sortOrder,
    });
    setTotalPages(Math.ceil(response.totalCount / pageSize));
  };

  useEffect(() => {
    fetchTasks();
  }, [pageNumber, pageSize, statusFilter, sortOrder]);

  if (loading) {
    return (
      <Center height="80vh">
        <Spinner size="xl" color="blue.500" thickness="4px" speed="0.65s" />
      </Center>
    );
  }

  return (
    <Box>
      <Flex
        justify="space-between"
        align="center"
        mb={4}
        flexWrap="wrap"
        gap={4}
      >
        <Button
          as={RouterLink}
          to="/tasks/add"
          colorScheme="teal"
          mb={4}
          alignSelf="flex-end"
        >
          Add New Task
        </Button>
        <Select
          width="150px"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value as Status | "")}
        >
          <option value="">All Statuses</option>
          <option value="0">Pending</option>
          <option value="1">InProgress</option>
          <option value="2">Completed</option>
          <option value="3">Cancelled</option>
        </Select>
        <Select
          width="150px"
          value={sortOrder}
          onChange={(e) => setSortOrder(e.target.value as "asc" | "desc")}
        >
          <option value="asc">Created (Asc)</option>
          <option value="desc">Created (Desc)</option>
        </Select>
        <Select
          width="100px"
          value={pageSize}
          onChange={(e) => setPageSize(Number(e.target.value))}
        >
          <option value={5}>5</option>
          <option value={10}>10</option>
          <option value={20}>20</option>
        </Select>
      </Flex>
      {tasks.length === 0 ? (
        <Text fontSize="lg" color="gray.500" textAlign="center" mt={10}>
          No tasks available 🚀
        </Text>
      ) : (
        <>
          <SimpleGrid columns={{ base: 1, md: 2, lg: 3 }} spacing={6}>
            {tasks.map((task) => {
              const IconComponent = statusIcons[task.status] || InfoIcon;
              const colorScheme = statusColors[task.status] || "gray";
              return (
                <Box
                  key={task.id}
                  p={6}
                  borderWidth="1px"
                  borderRadius="2xl"
                  shadow="lg"
                  bg={colorScheme + ".100"}
                  _hover={{
                    shadow: "xl",
                    transform: "translateY(-4px)",
                  }}
                  transition="all 0.2s ease-in-out"
                >
                  <VStack align="start" spacing={3}>
                    <HStack spacing={2} w="full">
                      <Icon
                        as={IconComponent}
                        w={6}
                        h={6}
                        color={`${colorScheme}.400`}
                      />
                      <Text fontWeight="bold" fontSize="xl" flex="1">
                        {task.title}
                      </Text>
                      <Badge colorScheme={colorScheme}>
                        {getStatusLabel(task.status)}
                      </Badge>
                    </HStack>

                    <Text color="gray.600">{task.description}</Text>

                    <HStack spacing={3} pt={2}>
                      <Button
                        as={RouterLink}
                        to={`/tasks/${task.id}`}
                        colorScheme="blue"
                        size="sm"
                        variant="outline"
                      >
                        View Details
                      </Button>

                      <Button
                        colorScheme="red"
                        size="sm"
                        variant="outline"
                        leftIcon={<DeleteIcon />}
                        onClick={() => removeTask(task.id)}
                      >
                        Delete
                      </Button>
                      <Button
                        as={RouterLink}
                        to={`/tasks/edit/${task.id}`}
                        colorScheme="yellow"
                        size="sm"
                        variant="outline"
                      >
                        Edit
                      </Button>
                    </HStack>
                  </VStack>
                </Box>
              );
            })}
          </SimpleGrid>
          <Flex justify="center" mt={8} gap={4} align="center">
            <Button
              onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
              disabled={pageNumber === 1}
            >
              Prev
            </Button>
            <Text>
              Page {pageNumber} of {totalPages}
            </Text>
            <Button
              onClick={() =>
                setPageNumber((prev) => Math.min(prev + 1, totalPages))
              }
              disabled={pageNumber === totalPages}
            >
              Next
            </Button>
          </Flex>
        </>
      )}
    </Box>
  );
};

export default TaskList;
