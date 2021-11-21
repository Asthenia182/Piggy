import { Box, Flex } from "@chakra-ui/react";
import React from "react";
import { NavBar } from "../components/NavBar";
const Index = () => {
  return (
    <>
      <Flex color="white">
        <Box w="200px" h="100vh" bg="#3A3D6A">
          <NavBar />
        </Box>
        <Box flex="1" h="100vh" p={10} bg="#F4F4FC"></Box>
      </Flex>
    </>
  );
};

export default Index;
