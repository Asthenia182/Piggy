using FluentValidation;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage(Consts.FIELD_REQUIRED)
            .MinimumLength(8)
            .WithMessage(Consts.INVALID_PASSWORD_LENGHT);
        RuleFor(user => user.UsernameOrEmail)
            .NotEmpty()
            .WithMessage(Consts.FIELD_REQUIRED);
    }



    public static bool IsEmail(string email)
    {
        if (email.Trim().EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}