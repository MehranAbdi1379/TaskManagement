import React, { useState } from "react";
import {
  Box,
  Button,
  Input,
  FormControl,
  FormLabel,
  VStack,
  Heading,
  Text,
  InputGroup,
  InputRightElement,
  FormErrorMessage,
  useToast,
} from "@chakra-ui/react";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Link as RouterLink, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../store/useAuthStore";

const LoginForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });
  const { register, login } = useAuthStore();

  const toast = useToast();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    // Basic validation
    const newErrors = { email: "", password: "", firstName: "", lastName: "" };
    if (!email) newErrors.email = "Email is required";
    if (!password) newErrors.password = "Password is required";
    if (!firstName) newErrors.firstName = "First name is required";
    if (!lastName) newErrors.lastName = "Last name is required";

    setErrors(newErrors);
    if (
      newErrors.email ||
      newErrors.password ||
      newErrors.firstName ||
      newErrors.lastName
    )
      return;

    setLoading(true);
    try {
      await register(email, password, firstName, lastName);

      toast({
        title: "Register successful",
        description: "Welcome!",
        status: "success",
        duration: 3000,
        isClosable: true,
      });

      await login({ username: email, password });

      navigate("/dashboard");
    } catch (error) {
      toast({
        title: "Register failed",
        description: error.message || "Something went wrong",
        status: "error",
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box
      maxW="400px"
      mx="auto"
      mt={10}
      p={6}
      borderRadius="2xl"
      shadow="lg"
      bg="white"
    >
      <Heading size="lg" textAlign="center" mb={4}>
        Sign Up
      </Heading>
      <VStack spacing={4}>
        <FormControl isInvalid={!!errors.firstName}>
          <FormLabel>First Name</FormLabel>
          <Input
            type="text"
            placeholder="Enter your first name"
            borderRadius="2xl"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />
          <FormErrorMessage>{errors.firstName}</FormErrorMessage>
        </FormControl>

        <FormControl isInvalid={!!errors.lastName}>
          <FormLabel>Last Name</FormLabel>
          <Input
            type="text"
            placeholder="Enter your last name"
            borderRadius="2xl"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />
          <FormErrorMessage>{errors.lastName}</FormErrorMessage>
        </FormControl>

        <FormControl isInvalid={!!errors.email}>
          <FormLabel>Email</FormLabel>
          <Input
            type="email"
            placeholder="Enter your email"
            borderRadius="2xl"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <FormErrorMessage>{errors.email}</FormErrorMessage>
        </FormControl>

        <FormControl isInvalid={!!errors.password}>
          <FormLabel>Password</FormLabel>
          <InputGroup>
            <Input
              type={showPassword ? "text" : "password"}
              placeholder="Enter your password"
              borderRadius="2xl"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <InputRightElement>
              <Button
                variant="ghost"
                onClick={() => setShowPassword(!showPassword)}
                size="sm"
              >
                {showPassword ? <ViewOffIcon /> : <ViewIcon />}
              </Button>
            </InputRightElement>
          </InputGroup>
          <FormErrorMessage>{errors.password}</FormErrorMessage>
        </FormControl>

        <Button
          colorScheme="teal"
          width="full"
          borderRadius="2xl"
          isLoading={loading}
          onClick={handleSubmit}
        >
          Sign Up
        </Button>

        <Text fontSize="sm">
          Already have an account?{" "}
          <RouterLink to="/login" style={{ color: "#3182ce" }}>
            Login
          </RouterLink>
        </Text>
      </VStack>
    </Box>
  );
};

export default LoginForm;
