import { ChakraProvider, ColorModeProvider } from "@chakra-ui/react";
import { Provider, createClient } from "urql";
import theme from "../theme";

const client = createClient({
  url: "https://localhost:7187/graphql/",
});

function MyApp({ Component, pageProps }) {
  return (
    <Provider value={client}>
      <ChakraProvider resetCSS theme={theme}>
        <ColorModeProvider
          options={{
            useSystemColorMode: true,
          }}
        >
          <Component {...pageProps} />
        </ColorModeProvider>
      </ChakraProvider>
    </Provider>
  );
}

export default MyApp;
