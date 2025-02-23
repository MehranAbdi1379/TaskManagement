import React, { useState } from "react";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  Select,
  Textarea,
  VStack,
  useToast,
} from "@chakra-ui/react";
import useTaskStore from "../../store/useTaskStore";
import { Status } from "../../api/taskApi";

const AddTask: React.FC = () => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [status, setStatus] = useState<Status>(Status.Pending);
  const { addTask } = useTaskStore();
  const toast = useToast();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) {
      toast({
        title: "Title is required.",
        status: "warning",
        isClosable: true,
      });
      return;
    }

    addTask({ title, description, status });
    toast({
      title: "Task added successfully!",
      status: "success",
      isClosable: true,
    });

    setTitle("");
    setDescription("");
    setStatus(Status.Pending);
  };

  return (
    <Box
      maxW="lg"
      mx="auto"
      mt={10}
      p={6}
      borderWidth="1px"
      borderRadius="xl"
      shadow="lg"
      bg="white"
    >
      <VStack spacing={4} align="stretch">
        <form onSubmit={handleSubmit} style={{ width: "100%" }}>
          <FormControl isRequired>
            <FormLabel>Title</FormLabel>
            <Input
              placeholder="Task title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            />
          </FormControl>

          <FormControl mt={4}>
            <FormLabel>Description</FormLabel>
            <Textarea
              placeholder="Task description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </FormControl>

          <FormControl mt={4}>
            <FormLabel>Status</FormLabel>
            <Select
              value={status}
              onChange={(e) => setStatus(Number(e.target.value))}
            >
              {Object.entries(Status)
                .filter(([key]) => isNaN(Number(key)))
                .map(([key, value]) => (
                  <option key={key} value={value}>
                    {key}
                  </option>
                ))}
            </Select>
          </FormControl>

          <Button mt={6} width="full" colorScheme="blue" type="submit">
            Add Task
          </Button>
        </form>
      </VStack>
    </Box>
  );
};

export default AddTask;
