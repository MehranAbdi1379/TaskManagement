import {Box, Button, Center, Flex, HStack, Select, SimpleGrid, Spinner, Text, VStack,} from "@chakra-ui/react";
import {useEffect, useState} from "react";
import {CheckIcon, CloseIcon} from "@chakra-ui/icons";
import useNotificationStore from "../../store/useNotificationStore";
import {useAuthStore} from "@/store/useAuthStore.ts";
import {AssignTaskToUserRespond} from "@/services/taskService.ts";


const NotificationList: React.FC = () => {
    const {
        notifications,
        getNotifications,
        updateNotificationIsRead,
        totalPages
    } = useNotificationStore();
    const loading = useNotificationStore((state) => state.loading);
    const {setIsAuthenticated} = useAuthStore();

    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");
    const [totalNotificationPages, setTotalNotificationPages] = useState(1);
    const [activeOrHistory, setActiveOrHistory] = useState<"active" | "history">(
        "active"
    );

    const fetchNotifications = async () => {
        try {
            await getNotifications({
                pageNumber,
                pageSize,
                sortOrder,
                isHistory: activeOrHistory == "history",
            });
            setTotalNotificationPages(totalPages);

        } catch {
            setIsAuthenticated(false);
        }
    };

    const handleNotificationUpdate = async (notificationId: number) => {
        // Make the API call to update the task status
        await updateNotificationIsRead(notificationId);
        await fetchNotifications();
    };

    const handleTaskAssignment = async (
        notificationId: number,
        accept: boolean
    ) => {
        await AssignTaskToUserRespond(notificationId, accept);
        await fetchNotifications();
    };

    useEffect(() => {
        fetchNotifications();
    }, [pageNumber, pageSize, sortOrder, activeOrHistory, fetchNotifications]);

    if (loading) {
        return (
            <Center height="80vh">
                <Spinner size="xl" color="blue.500" thickness="4px" speed="0.65s"/>
            </Center>
        );
    }

    return (
        <Box>
            <Flex justify="center" align="center" mb={4} wrap="wrap" gap={4}>
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
                >
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                </Select>
                <Select
                    width="180px"
                    value={activeOrHistory}
                    onChange={(e) =>
                        setActiveOrHistory(
                            e.target.value == ""
                                ? "active"
                                : (e.target.value as "active" | "history")
                        )
                    }
                    placeholder="Sort By"
                    borderRadius="2xl"
                    backgroundColor={activeOrHistory == "active" ? "red.100" : "blue.100"}
                >
                    <option value="active">Active</option>
                    <option value="history">History</option>
                </Select>
            </Flex>

            {notifications.length === 0 ? (
                <Text fontSize="lg" color="gray.500" textAlign="center" mt={10}>
                    No notifications available 📩
                </Text>
            ) : (
                <SimpleGrid columns={{base: 1, md: 2, lg: 3}} spacing={6}>
                    {notifications.map((notification) => (
                        <Box
                            key={notification.id}
                            p={6}
                            borderWidth="1px"
                            borderRadius="2xl"
                            shadow="lg"
                            bg={notification.isRead ? "gray.100" : "blue.100"}
                        >
                            <VStack align="start" spacing={3}>
                                <HStack spacing={2} w="full">
                                    <Text fontWeight="bold" fontSize="xl" flex="1">
                                        {notification.title}
                                    </Text>
                                </HStack>
                                <Text color="gray.600">{notification.content}</Text>
                                <HStack spacing={3} pt={2}>
                                    {activeOrHistory == "active" ? (
                                        <>
                                            {notification.type === "TaskAssignmentRequest" ? (
                                                <>
                                                    <Button
                                                        colorScheme="green"
                                                        size="sm"
                                                        onClick={() =>
                                                            handleTaskAssignment(notification.id, true)
                                                        }
                                                        leftIcon={<CheckIcon/>}
                                                    >
                                                        Accept
                                                    </Button>
                                                    <Button
                                                        colorScheme="red"
                                                        size="sm"
                                                        onClick={() =>
                                                            handleTaskAssignment(notification.id, false)
                                                        }
                                                        leftIcon={<CloseIcon/>}
                                                    >
                                                        Reject
                                                    </Button>
                                                </>
                                            ) : (
                                                <Button
                                                    colorScheme="blue"
                                                    size="sm"
                                                    onClick={() =>
                                                        handleNotificationUpdate(notification.id)
                                                    }
                                                >
                                                    Mark as Read
                                                </Button>
                                            )}
                                        </>
                                    ) : (
                                        <></>
                                    )}
                                </HStack>
                            </VStack>
                        </Box>
                    ))}
                </SimpleGrid>
            )}

            {totalNotificationPages != 0 && (
                <Flex justify="center" mt={8} gap={4} align="center">
                    <Button
                        onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
                        disabled={pageNumber === 1}
                    >
                        Prev
                    </Button>
                    <Text>
                        Page {pageNumber} of {totalNotificationPages}
                    </Text>
                    <Button
                        onClick={() =>
                            setPageNumber((prev) => Math.min(prev + 1, totalNotificationPages))
                        }
                        disabled={pageNumber === totalNotificationPages}
                    >
                        Next
                    </Button>
                </Flex>
            )}
        </Box>
    );
};

export default NotificationList;
