using FluentValidation;
using MongoDB.Driver;

public class UserInputValidator : AbstractValidator<UserInput>
{
    IMongoCollection<User>? _users;

    public UserInputValidator(IMongoCollection<User> users)
    {
        _users = users;

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage(Consts.FIELD_REQUIRED)
            .MinimumLength(8)
            .WithMessage(Consts.INVALID_PASSWORD_LENGHT);
        RuleFor(user => user.UsernameOrEmail)
            .NotEmpty()
            .WithMessage(Consts.FIELD_REQUIRED);

        if (_users != null)
            RuleFor(user => user.UsernameOrEmail)
                .Must(UniqueUserAsync)
                .WithMessage(Consts.USER_EXISTS);
    }

    private bool UniqueUserAsync(string userNameOrPassword)
    {
        return !UserExists(userNameOrPassword);
    }

    private bool UserExists(string userNameOrPassword)
    {
        string? email = IsEmail(userNameOrPassword) ? userNameOrPassword : null;
        User? user = email != null
            ? _users.Find(x => x.Email == userNameOrPassword).FirstOrDefault()
            : _users.Find(x => x.Username == userNameOrPassword).FirstOrDefault();

        return user != null;
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