import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  VStack,
  Text,
  Input,
  HStack,
  Divider,
  IconButton,
  useToast,
  Spinner,
  Select,
} from "@chakra-ui/react";
import { DeleteIcon } from "@chakra-ui/icons";
import useTaskCommentStore from "../../store/useCommentStore";

interface TaskCommentsProps {
  taskId: number;
}

const TaskComments: React.FC<TaskCommentsProps> = ({ taskId }) => {
  const toast = useToast();
  const {
    comments,
    loading,
    getTaskComments,
    addTaskComment,
    removeTaskComment,
  } = useTaskCommentStore();
  const [newComment, setNewComment] = useState("");
  const [commentOwner, setCommentOwner] = useState(false);
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

  useEffect(() => {
    getTaskComments(taskId, {
      pageNumber: 1,
      pageSize: 10,
      commentOwner,
      sortOrder: sortOrder,
    });
  }, [taskId, commentOwner, getTaskComments]);

  const handleAddComment = async () => {
    if (!newComment.trim()) {
      toast({
        title: "Comment cannot be empty.",
        status: "warning",
        isClosable: true,
      });
      return;
    }
    await addTaskComment(newComment, taskId);
    setNewComment("");
    toast({ title: "Comment added!", status: "success", isClosable: true });
  };

  const handleDeleteComment = async (commentId: number) => {
    await removeTaskComment(commentId);
    toast({ title: "Comment deleted!", status: "info", isClosable: true });
  };

  return (
    <Box
      width="100%"
      mt={6}
      p={4}
      borderWidth="1px"
      borderRadius="md"
      shadow="md"
      bg="gray.50"
    >
      <HStack justifyContent="space-between" mb={4}>
        <Text fontSize="lg" fontWeight="bold">
          Comments
        </Text>
        <Select
          _hover={{ shadow: "md" }}
          _focus={{ borderColor: "teal.500", shadow: "lg" }}
          value={commentOwner ? "mine" : "all"}
          onChange={(e) => setCommentOwner(e.target.value === "mine")}
        >
          <option value="all">All Comments</option>
          <option value="mine">My Comments</option>
        </Select>
        <Select
          value={sortOrder}
          onChange={(e) => setSortOrder(e.target.value as "asc" | "desc")}
          _hover={{ shadow: "md" }}
          _focus={{ borderColor: "teal.500", shadow: "lg" }}
        >
          <option value="asc">Oldest</option>
          <option value="desc">Newest</option>
        </Select>
      </HStack>

      {loading ? (
        <Spinner />
      ) : comments.length === 0 ? (
        <Text color="gray.500">No comments yet.</Text>
      ) : (
        <VStack spacing={3} align="start">
          {comments.map((comment) => (
            <Box
              key={comment.id}
              w="100%"
              p={3}
              borderWidth="1px"
              borderRadius="md"
              bg="white"
            >
              <HStack justifyContent="space-between">
                <Text fontSize="sm" fontWeight="bold">
                  {comment.userFullName}
                </Text>
                {comment.isOwner && (
                  <IconButton
                    size="sm"
                    aria-label="Delete comment"
                    icon={<DeleteIcon />}
                    colorScheme="red"
                    variant="outline"
                    onClick={() => handleDeleteComment(comment.id)}
                  />
                )}
              </HStack>
              <Text mt={2}>{comment.text}</Text>
              <Text fontSize="xs" color="gray.500">
                {new Date(comment.createdAt).toLocaleString()}
              </Text>
            </Box>
          ))}
        </VStack>
      )}
      <Divider my={4} />
      <HStack>
        <Input
          placeholder="Write a comment..."
          value={newComment}
          onChange={(e) => setNewComment(e.target.value)}
        />
        <Button colorScheme="blue" onClick={handleAddComment}>
          Add
        </Button>
      </HStack>
    </Box>
  );
};

export default TaskComments;
