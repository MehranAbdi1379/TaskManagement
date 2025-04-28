import React, { useEffect, useRef, useState } from "react";
import { useParams, Link as RouterLink, useNavigate } from "react-router-dom";
import {
  Box,
  Button,
  Text,
  Badge,
  VStack,
  HStack,
  Icon,
  useDisclosure,
  AlertDialog,
  AlertDialogBody,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogContent,
  AlertDialogOverlay,
  FormControl,
  FormLabel,
  Input,
  useToast,
} from "@chakra-ui/react";
import { InfoIcon, ArrowBackIcon, DeleteIcon } from "@chakra-ui/icons";
import useTaskStore from "../../store/useTaskStore";
import { getStatusLabel, statusColors, statusIcons } from "./Task";
import { format } from "date-fns";
import { AssignTaskToUserRequest } from "../../api/taskApi";
import TaskComments from "./TaskComments";
import { joinGroup, leaveGroup } from "../../store/signalRGroupManager";

const TaskDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [assignUserToTaskOpen, setAssignUserToTaskOpen] = useState(false);
  const [userEmailToAssign, setUserEmailToAssign] = useState("");
  const navigate = useNavigate();
  const { getTaskById } = useTaskStore();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const toast = useToast();
  const cancelRef = useRef<HTMLButtonElement>(
    null
  ) as React.RefObject<HTMLButtonElement>;

  const task = useTaskStore((state) =>
    state.tasks.find((t) => t.id === Number(id))
  );

  useEffect(() => {
    if (!task) {
      getTaskById(Number(id));
    }
    if (task) {
      joinGroup(`Task-${task.id}`);
    }

    return () => {
      if (task) {
        leaveGroup(`Task-${task.id}`);
      }
    };
  }, [id, task, getTaskById]);

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
  const formattedDate = format(
    new Date(task.createdAt),
    "MMMM dd, yyyy h:mm a"
  );

  const handleAssignUser = async () => {
    try {
      await AssignTaskToUserRequest(userEmailToAssign, task.id);
      toast({
        title: "Request Has Been Sent!!!",
        status: "success",
        isClosable: true,
      });
      setAssignUserToTaskOpen(false);
    } catch {
      toast({
        title: "Please check the Email!!!",
        status: "warning",
        isClosable: true,
      });
    }
  };

  const handleDelete = () => {
    useTaskStore.getState().removeTask(task.id); // Remove task
    onClose(); // Close the dialog
    navigate("/"); // Navigate back to tasks
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

        <Box w="100%" bg="gray.50" p={4} borderRadius="md" borderWidth="1px">
          <Text fontSize="lg" fontWeight="semibold" mb={2}>
            Created At:
          </Text>
          <Text color="gray.700">{formattedDate}</Text>
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

          {task.isOwner && (
            <>
              <Button
                colorScheme="red"
                variant="outline"
                leftIcon={<DeleteIcon />}
                onClick={onOpen} // Open the confirmation dialog
                _hover={{ bg: "red.50" }}
              >
                Delete
              </Button>

              <Button
                colorScheme={assignUserToTaskOpen ? "blackAlpha" : "green"}
                variant="outline"
                _hover={{ bg: "green.50" }}
                onClick={() => setAssignUserToTaskOpen(!assignUserToTaskOpen)}
              >
                Assign User To Task
              </Button>
            </>
          )}

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

      {assignUserToTaskOpen && (
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
          <VStack spacing={4} align="stretch">
            <FormControl isRequired>
              <FormLabel>User Email</FormLabel>
              <Input
                placeholder="User Email"
                value={userEmailToAssign}
                onChange={(e) => setUserEmailToAssign(e.target.value)}
              />
            </FormControl>

            <Button
              onClick={() => handleAssignUser()}
              mt={6}
              width="full"
              colorScheme="teal"
              type="submit"
            >
              Assign User
            </Button>
          </VStack>
        </Box>
      )}

      {!assignUserToTaskOpen && <TaskComments taskId={task.id} />}

      {/* AlertDialog for confirmation */}
      <AlertDialog
        isOpen={isOpen}
        leastDestructiveRef={cancelRef}
        onClose={onClose}
        isCentered
      >
        <AlertDialogOverlay>
          <AlertDialogContent>
            <AlertDialogHeader fontSize="lg" fontWeight="bold">
              Confirm Deletion
            </AlertDialogHeader>

            <AlertDialogBody>
              Are you sure you want to delete this task? This action cannot be
              undone.
            </AlertDialogBody>

            <AlertDialogFooter>
              <Button ref={cancelRef} onClick={onClose}>
                No
              </Button>
              <Button colorScheme="red" onClick={handleDelete} ml={3}>
                Yes
              </Button>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialogOverlay>
      </AlertDialog>
    </Box>
  );
};

export default TaskDetail;
