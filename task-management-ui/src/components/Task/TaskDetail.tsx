import React from "react";
import { useParams, Link as RouterLink, useNavigate } from "react-router-dom";
import {
  Box,
  Button,
  Text,
  Badge,
  VStack,
  HStack,
  Icon,
} from "@chakra-ui/react";
import { InfoIcon, ArrowBackIcon, DeleteIcon } from "@chakra-ui/icons";
import useTaskStore from "../../store/useTaskStore";
import { getStatusLabel, statusColors, statusIcons } from "./Task";

const TaskDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate(); // To navigate after task deletion
  const task = useTaskStore((state) =>
    state.tasks.find((t) => t.id === Number(id))
  );

  if (!task) {
    return (
      <Box textAlign="center" mt={10}>
        <Text fontSize="xl" color="red.500">
          🚫 Task not found.
        </Text>
        <Button
          as={RouterLink}
          to="/"
          leftIcon={<ArrowBackIcon />}
          mt={4}
          colorScheme="blue"
        >
          Back to Tasks
        </Button>
      </Box>
    );
  }

  const IconComponent = statusIcons[task.status] || InfoIcon;
  const colorScheme = statusColors[task.status] || "gray";

  const handleDelete = () => {
    useTaskStore.getState().removeTask(task.id); // Removing the task
    navigate("/"); // Navigate to the main page
  };

  return (
    <Box
      maxW="4xl"
      width="100%"
      mx="auto"
      p={8}
      borderWidth="1px"
      borderRadius="2xl"
      shadow="xl"
      bg={`${colorScheme}.50`}
      mt={8}
      _hover={{ shadow: "2xl" }}
      transition="all 0.3s ease"
    >
      <VStack spacing={6} align="start" width="100%">
        <HStack spacing={3} width="100%">
          <Icon as={IconComponent} w={8} h={8} color={`${colorScheme}.400`} />
          <Text
            fontSize="3xl"
            fontWeight="bold"
            flex="1"
            isTruncated
            whiteSpace="nowrap"
            maxW="calc(100% - 140px)"
          >
            {task.title}
          </Text>
          <Badge
            fontSize="md"
            colorScheme={colorScheme}
            borderRadius="full"
            px={3}
            py={1}
          >
            {getStatusLabel(task.status)}
          </Badge>
        </HStack>

        <Box w="100%" bg="gray.50" p={4} borderRadius="md" borderWidth="1px">
          <Text fontSize="lg" fontWeight="semibold" mb={2}>
            Description:
          </Text>
          <Text color="gray.700">{task.description}</Text>
        </Box>

        <HStack spacing={4} pt={4}>
          <Button
            as={RouterLink}
            to="/"
            leftIcon={<ArrowBackIcon />}
            colorScheme="blue"
            variant="outline"
            _hover={{ bg: "blue.50" }}
          >
            Back to Tasks
          </Button>

          <Button
            colorScheme="red"
            variant="outline"
            leftIcon={<DeleteIcon />}
            onClick={handleDelete}
            _hover={{ bg: "red.50" }}
          >
            Delete
          </Button>
          <Button
            as={RouterLink}
            to={`/tasks/edit/${task.id}`}
            colorScheme="yellow"
            variant="outline"
            _hover={{ bg: "yellow.50" }}
          >
            Edit
          </Button>
        </HStack>
      </VStack>
    </Box>
  );
};

export default TaskDetail;
