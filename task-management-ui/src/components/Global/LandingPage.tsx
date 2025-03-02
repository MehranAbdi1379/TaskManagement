import { Button, Center, Heading, Text, VStack } from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";

const LandingPage: React.FC = () => {
  return (
    <Center height="100vh" bg="gray.100">
      <VStack spacing={6} p={8} bg="white" borderRadius="lg" boxShadow="xl">
        <Heading size="xl" color="blue.600">
          Welcome to Task Manager
        </Heading>
        <Text fontSize="lg" textAlign="center" color="gray.600">
          Organize your tasks efficiently and boost your productivity.
        </Text>
        <VStack spacing={4} width="100%">
          <Button
            as={RouterLink}
            to="/login"
            colorScheme="blue"
            size="lg"
            width="100%"
          >
            Login
          </Button>
          <Button
            as={RouterLink}
            to="/register"
            colorScheme="teal"
            size="lg"
            width="100%"
          >
            Sign Up
          </Button>
        </VStack>
      </VStack>
    </Center>
  );
};

export default LandingPage;
