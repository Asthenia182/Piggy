import React from "react";
import { Formik, Form } from "formik";
import { Wrapper } from "../components/Wrapper";
import { InputField } from "../components/InputField";
import { Box, Center } from "@chakra-ui/layout";
import { Button } from "@chakra-ui/button";
import { useRouter } from "next/dist/client/router";
import { useRegisterMutation } from "../generated/graphql";
import { toErrorMap } from "../utils/toErrorMap";

interface registerProps {}

const Register: React.FC<registerProps> = ({}) => {
  const router = useRouter();

  const [, register] = useRegisterMutation();

  return (
    <Center h="100vh" bg="#1A1D43" color="#EFF1ED">
      <Wrapper variant="small">
        <Formik
          initialValues={{ usernameOrEmail: "", password: "" }}
          onSubmit={async (values, { setErrors }) => {
            const response = await register({ input: values });
            if (response.data?.register.fieldErrors) {
              setErrors(toErrorMap(response.data.register.fieldErrors));
            } else if (response.data?.register.usernameOrEmail) {
              router.push("/");
            }
          }}
        >
          {({ isSubmitting }) => (
            <Form>
              <InputField
                focusBorderColor="#EFF1ED"
                color="red"
                variant="filled"
                name="usernameOrEmail"
                label="USERNAME OR EMAIL"
                placeholder="username or email"
              />
              <Box mt={4}>
                <InputField
                  focusBorderColor="#EFF1ED"
                  variant="filled"
                  name="password"
                  label="PASSWORD"
                  placeholder="password"
                  type="password"
                />
              </Box>
              <Button
                mt={8}
                type="submit"
                colorScheme="#EFF1ED"
                variant="outline"
                width="400px"
                isLoading={isSubmitting}
              >
                Register
              </Button>
            </Form>
          )}
        </Formik>
      </Wrapper>
    </Center>
  );
};

export default Register;
