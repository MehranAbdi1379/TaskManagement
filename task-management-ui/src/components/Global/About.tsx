import React from "react";
import { Box, Heading, Text, VStack, Button } from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";

const About: React.FC = () => {
  return (
    <Box maxW="4xl" mx="auto" p={8}>
      <VStack spacing={6} align="start">
        <Heading size="2xl" color="blue.600">
          About Task Manager Pro
        </Heading>
        <Text fontSize="lg" color="gray.700">
          Task Manager Pro is a simple yet powerful task management application
          designed to help you organize your daily activities efficiently.
          Whether you're managing personal projects or work-related tasks, our
          application provides the tools you need to stay productive.
        </Text>

        <Text fontSize="lg" color="gray.700">
          🚀 <strong>Features:</strong>
        </Text>
        <ul style={{ paddingLeft: "1.5rem", marginTop: "0.5rem" }}>
          <li>Create, update, and delete tasks easily.</li>
          <li>Track task status to stay on top of your work.</li>
          <li>Responsive design for all devices.</li>
          <li>Beautiful and intuitive user interface.</li>
        </ul>

        <Text fontSize="lg" color="gray.700">
          This project is built with ❤️ using React, Chakra UI, Zustand for
          state management, and React Router for seamless navigation.
        </Text>

        <Button as={RouterLink} to="/" colorScheme="blue" size="lg" mt={4}>
          Go to Tasks
        </Button>
      </VStack>
    </Box>
  );
};

export default About;
