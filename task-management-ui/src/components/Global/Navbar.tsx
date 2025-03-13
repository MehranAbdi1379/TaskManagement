import {
  Flex,
  Heading,
  HStack,
  Spacer,
  Button,
  Link,
  Box,
} from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";
import { FiHome, FiInfo, FiLogIn, FiLogOut } from "react-icons/fi";
import { useAuthStore } from "../../store/useAuthStore";
import { BellIcon } from "@chakra-ui/icons";

const Navbar: React.FC = () => {
  const { isAuthenticated, logout } = useAuthStore();
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
        <Link as={RouterLink} to="/" _hover={{ textDecoration: "none" }}>
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
          _hover={{ textDecoration: "none", color: "blue.200" }}
        >
          <Box as={FiHome} /> Home
        </Link>
        <Link
          as={RouterLink}
          to="/about"
          display="flex"
          alignItems="center"
          gap={2}
          _hover={{ textDecoration: "none", color: "blue.200" }}
        >
          <Box as={FiInfo} /> About
        </Link>
      </HStack>

      <Spacer />

      {!isAuthenticated && (
        <>
          <Button
            leftIcon={<FiLogIn />}
            colorScheme="green"
            variant="solid"
            size="md"
            _hover={{ bg: "green.300" }}
            marginRight={5}
          >
            Register
          </Button>
          <Button
            leftIcon={<FiLogIn />}
            colorScheme="green"
            variant="solid"
            size="md"
            _hover={{ bg: "green.300" }}
          >
            Login
          </Button>
        </>
      )}
      {isAuthenticated && (
        <>
          <Button
            leftIcon={<FiLogOut />}
            as={RouterLink}
            to={`/`}
            colorScheme="red"
            variant="solid"
            marginRight={4}
            size="md"
            _hover={{ bg: "red.300" }}
            onClick={() => {
              logout();
            }}
          >
            Logout
          </Button>
          <Button
            leftIcon={<BellIcon />}
            as={RouterLink}
            to={`/notifications`}
            colorScheme="green"
            variant="solid"
            size="md"
            _hover={{ bg: "green.300" }}
          >
            Notifications
          </Button>
        </>
      )}
    </Flex>
  );
};

export default Navbar;
