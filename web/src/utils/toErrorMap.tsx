import { FieldError } from "../generated/graphql";
import { lowerFirstLetter } from "./toLowerFirstLetter";

export const toErrorMap = (errors: FieldError[]) => {
  const errorMap: Record<string, string> = {};
  errors.forEach(({ name, message }) => {
    errorMap[lowerFirstLetter(name)] = message;
  });

  return errorMap;
};
