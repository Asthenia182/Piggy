import { Box, Center } from "@chakra-ui/layout";
import React from "react";
import NextLink from "next/link";
import { useRouter } from "next/dist/client/router";

interface NavBarProps {}

export const NavBar: React.FC<NavBarProps> = ({}) => {
  const router = useRouter();

  return (
    <Box>
      <NextLink href="/">
        <Center
          p="2"
          layerStyle={router.pathname == "/dashboard" ? "navSelected" : "nav"}
        >
          Pulpit
        </Center>
      </NextLink>
      <NextLink href="/">
        <Center
          p="2"
          layerStyle={router.pathname == "/outcomes" ? "navSelected" : "nav"}
        >
          Wydatki
        </Center>
      </NextLink>
      <NextLink href="/">
        <Center
          p="2"
          layerStyle={router.pathname == "/incomes" ? "navSelected" : "nav"}
        >
          Przychody
        </Center>
      </NextLink>
      <NextLink href="/">
        <Center
          p="2"
          layerStyle={router.pathname == "/goals" ? "navSelected" : "nav"}
        >
          Cele
        </Center>
      </NextLink>
      <NextLink href="/">
        <Center
          p="2"
          layerStyle={router.pathname == "/prediction" ? "navSelected" : "nav"}
        >
          Prognoza
        </Center>
      </NextLink>
    </Box>
  );
};
