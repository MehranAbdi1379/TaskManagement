import React, {useEffect, useState} from "react";
import {
    Badge,
    Box,
    Button,
    Center,
    Flex,
    HStack,
    Icon,
    Menu,
    MenuButton,
    MenuItem,
    MenuList,
    Select,
    SimpleGrid,
    Spinner,
    Text,
    VStack,
} from "@chakra-ui/react";
import {Link as RouterLink} from "react-router-dom";
import {ChevronDownIcon, InfoIcon} from "@chakra-ui/icons";
import useTaskStore from "../../store/useTaskStore";
import {getStatusLabel, statusColors, statusIcons} from "./Task";

import {useAuthStore} from "@/store/useAuthStore.ts";
import {Status} from "@/models/task.ts";
import {AxiosError} from "axios";

const TaskList: React.FC = () => {
    const {tasks, getTasks, updateTaskStatus, totalPages} = useTaskStore();
    const loading = useTaskStore((state) => state.loading);
    const {setIsAuthenticated} = useAuthStore();

    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [statusFilter, setStatusFilter] = useState<Status | "">("");
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");
    const [taskTotalPages, setTaskTotalPages] = useState(1);

    const fetchTasks = async () => {
        try {
            await getTasks({
                pageNumber,
                pageSize,
                status: statusFilter || undefined,
                sortOrder,
            });
            setTaskTotalPages(totalPages);
        } catch (error: unknown) {
            const axiosError = error as AxiosError;

            if (axiosError?.response?.status === 401) {
                setIsAuthenticated(false);
            } else {
                console.error("Unexpected error fetching tasks:", error);
            }
        }
    };

    const handleStatusUpdate = async (taskId: number, newStatus: Status) => {
        // Make the API call to update the task status
        await updateTaskStatus(taskId, newStatus);
    };

    useEffect(() => {
        fetchTasks();
    }, [pageNumber, pageSize, statusFilter, sortOrder]);

    if (loading) {
        return (
            <Center height="80vh">
                <Spinner size="xl" color="blue.500" thickness="4px" speed="0.65s"/>
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
                    shadow="md"
                    borderRadius="2xl"
                    _hover={{shadow: "lg", transform: "translateY(-2px)"}}
                >
                    Add New Task
                </Button>

                <Select
                    width="180px"
                    value={statusFilter}
                    onChange={(e) => setStatusFilter(e.target.value as Status | "")}
                    placeholder="Select Status"
                    borderRadius="2xl"
                    shadow="sm"
                    bg="white"
                    _hover={{shadow: "md"}}
                    _focus={{borderColor: "teal.500", shadow: "lg"}}
                >
                    <option value="">All Statuses</option>
                    <option value="0">Pending</option>
                    <option value="1">InProgress</option>
                    <option value="2">Completed</option>
                    <option value="3">Cancelled</option>
                </Select>

                <Select
                    width="180px"
                    value={sortOrder}
                    onChange={(e) =>
                        setSortOrder(
                            e.target.value == "" ? "asc" : (e.target.value as "asc" | "desc")
                        )
                    }
                    placeholder="Sort By"
                    borderRadius="2xl"
                    shadow="sm"
                    bg="white"
                    _hover={{shadow: "md"}}
                    _focus={{borderColor: "teal.500", shadow: "lg"}}
                >
                    <option value="asc">Oldest</option>
                    <option value="desc">Newest</option>
                </Select>

                <Select
                    width="130px"
                    value={pageSize}
                    onChange={(e) => setPageSize(Number(e.target.value))}
                    placeholder="Page Size"
                    borderRadius="2xl"
                    shadow="sm"
                    bg="white"
                    _hover={{shadow: "md"}}
                    _focus={{borderColor: "teal.500", shadow: "lg"}}
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
                    <SimpleGrid columns={{base: 1, md: 2, lg: 3}} spacing={6}>
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

                                            <Menu>
                                                <MenuButton
                                                    as={Button}
                                                    rightIcon={<ChevronDownIcon/>}
                                                    colorScheme={colorScheme}
                                                    size="sm"
                                                >
                                                    Update Status
                                                </MenuButton>
                                                <MenuList>
                                                    {["0", "1", "2", "3"].map((status) => (
                                                        <MenuItem
                                                            key={status}
                                                            onClick={() =>
                                                                handleStatusUpdate(
                                                                    task.id,
                                                                    parseInt(status) as Status
                                                                )
                                                            }
                                                        >
                                                            {getStatusLabel(parseInt(status) as Status)}
                                                        </MenuItem>
                                                    ))}
                                                </MenuList>
                                            </Menu>
                                        </HStack>
                                    </VStack>
                                </Box>
                            );
                        })}
                    </SimpleGrid>
                    {taskTotalPages != 0 && taskTotalPages > 0 && (
                        <>
                            <Flex justify="center" mt={8} gap={4} align="center">
                                <Button
                                    onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
                                    disabled={pageNumber === 1}
                                >
                                    Prev
                                </Button>
                                <Text>
                                    Page {pageNumber} of {taskTotalPages}
                                </Text>
                                <Button
                                    onClick={() =>
                                        setPageNumber((prev) => Math.min(prev + 1, taskTotalPages))
                                    }
                                    disabled={pageNumber === taskTotalPages}
                                >
                                    Next
                                </Button>
                            </Flex>
                        </>
                    )}
                </>
            )}
        </Box>
    );
};

export default TaskList;
