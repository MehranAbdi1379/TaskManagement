import {Badge, Box, Button, Flex, Heading, HStack, Link, Spacer,} from "@chakra-ui/react";
import {Link as RouterLink} from "react-router-dom";
import {FiHome, FiInfo, FiLogIn, FiLogOut} from "react-icons/fi";
import {useAuthStore} from "../../store/useAuthStore";
import {BellIcon} from "@chakra-ui/icons";
import useNotificationStore from "../../store/useNotificationStore";
import {useEffect} from "react";

const Navbar: React.FC = () => {
    const {isAuthenticated, logout} = useAuthStore();
    const {getNotifications, notifications} = useNotificationStore();
    const notificationActiveOrHistory = useNotificationStore(
        (state) => state.notificationActiveOrHistory
    );

    useEffect(() => {
        if (isAuthenticated) {
            getNotifications({
                pageNumber: 1,
                pageSize: 99,
                sortOrder: "desc",
                isHistory: false
            });
        }
    }, [isAuthenticated, getNotifications]);
    return (
        <Flex
            as="nav"
            bg="blue.600"
            color="white"
            py={4}
            px={8}
            align="center"
            shadow="md"
            borderBottom="3px solid"
            borderColor="blue.700"
        >
            <Heading
                size="lg"
                fontWeight="bold"
                letterSpacing="wide"
                color="whiteAlpha.900"
            >
                <Link as={RouterLink} to="/" _hover={{textDecoration: "none"}}>
                    Task Manager
                </Link>
            </Heading>

            <HStack spacing={6} ml={10} fontSize="md">
                <Link
                    as={RouterLink}
                    to="/"
                    display="flex"
                    alignItems="center"
                    gap={2}
                    _hover={{textDecoration: "none", color: "blue.200"}}
                >
                    <Box as={FiHome}/> Home
                </Link>
                <Link
                    as={RouterLink}
                    to="/about"
                    display="flex"
                    alignItems="center"
                    gap={2}
                    _hover={{textDecoration: "none", color: "blue.200"}}
                >
                    <Box as={FiInfo}/> About
                </Link>
            </HStack>

            <Spacer/>

            {!isAuthenticated && (
                <>
                    <Button
                        leftIcon={<FiLogIn/>}
                        colorScheme="green"
                        variant="solid"
                        size="md"
                        _hover={{bg: "green.300"}}
                        marginRight={5}
                    >
                        Register
                    </Button>
                    <Button
                        leftIcon={<FiLogIn/>}
                        colorScheme="green"
                        variant="solid"
                        size="md"
                        _hover={{bg: "green.300"}}
                    >
                        Login
                    </Button>
                </>
            )}
            {isAuthenticated && (
                <>
                    <Button
                        leftIcon={<FiLogOut/>}
                        as={RouterLink}
                        to={`/`}
                        colorScheme="red"
                        variant="solid"
                        marginRight={4}
                        size="md"
                        _hover={{bg: "red.300"}}
                        onClick={() => {
                            logout();
                        }}
                    >
                        Logout
                    </Button>
                    <Button
                        as={RouterLink}
                        to={`/notifications`}
                        colorScheme="green"
                        variant="solid"
                        size="md"
                        _hover={{bg: "green.300"}}
                        leftIcon={
                            <Flex position="relative">
                                <BellIcon/>
                                {notifications.length > 0 &&
                                    notificationActiveOrHistory == "active" && (
                                        <Badge
                                            position="absolute"
                                            top="2.5"
                                            right="2.5"
                                            fontSize="0.7em"
                                            colorScheme="red"
                                            borderRadius="full"
                                            px="0.5"
                                            py="0.3"
                                        >
                                            {notifications.length}
                                        </Badge>
                                    )}
                            </Flex>
                        }
                    >
                        Notifications
                    </Button>
                </>
            )}
        </Flex>
    );
};

export default Navbar;
