import React, {useEffect, useState} from "react";
import {Link as RouterLink, useNavigate, useParams} from "react-router-dom";
import {
    Box,
    Button,
    FormControl,
    FormLabel,
    HStack,
    Input,
    Select,
    Text,
    Textarea,
    useToast,
    VStack,
} from "@chakra-ui/react";
import useTaskStore from "../../store/useTaskStore";

import {ArrowBackIcon} from "@chakra-ui/icons";
import {Status} from "@/models/task.ts";

const UpdateTask: React.FC = () => {
    const {id} = useParams<{ id: string }>();
    const navigate = useNavigate();
    const {tasks, updateTask} = useTaskStore();
    const toast = useToast();
    const task = tasks.find((t) => t.id === Number(id));

    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [status, setStatus] = useState<Status>(Status.Pending);

    useEffect(() => {
        if (task) {
            setTitle(task.title);
            setDescription(task.description);
            setStatus(task.status);
        }
    }, [task]);

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

        updateTask({id: task!.id, title, description, status, isOwner: true});
        toast({
            title: "Task updated successfully!",
            status: "success",
            isClosable: true,
        });
        navigate(`/tasks/${task!.id}`);
    };

    if (!task) return <Text textAlign="center">Task not found.</Text>;

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
                <form onSubmit={handleSubmit} style={{width: "100%"}}>
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

                    <HStack>
                        <Button
                            as={RouterLink}
                            to={`/tasks/${task.id}`}
                            variant="outline"
                            leftIcon={<ArrowBackIcon/>}
                            colorScheme="blue"
                            mt={6}
                            width="50%"
                        >
                            Back to Task
                        </Button>

                        <Button mt={6} width="full" colorScheme="teal" type="submit">
                            Update Task
                        </Button>
                    </HStack>
                </form>
            </VStack>
        </Box>
    );
};

export default UpdateTask;
