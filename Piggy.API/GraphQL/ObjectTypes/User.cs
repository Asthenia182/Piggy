public record UserInput(string UsernameOrEmail, string Password);

public record UserPayload(string UsernameOrEmail, FieldError[]? fieldErrors);