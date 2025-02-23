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
import { FiHome, FiInfo, FiLogIn } from "react-icons/fi";

const Navbar: React.FC = () => {
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

      <Button
        leftIcon={<FiLogIn />}
        colorScheme="teal"
        variant="solid"
        size="md"
        _hover={{ bg: "teal.500" }}
      >
        Login
      </Button>
    </Flex>
  );
};

export default Navbar;
