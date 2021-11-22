using FluentValidation;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage(ValidationErrors.FIELD_REQUIRED)
            .MinimumLength(8)
            .WithMessage(ValidationErrors.INVALID_PASSWORD_LENGHT);
        RuleFor(user => user.UsernameOrEmail)
            .NotEmpty()
            .WithMessage(ValidationErrors.FIELD_REQUIRED);
    }
}