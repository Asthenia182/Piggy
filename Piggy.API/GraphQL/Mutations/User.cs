using MongoDB.Driver;

public partial class Mutation
{
    public async Task<UserPayload> RegisterAsync(
      [Service] IMongoDatabase db,
      UserInput input)
    {
        var users = db.GetCollection<User>("Users");
        var result = new UserInputValidator(users).Validate(input);
        if (!result.IsValid)
        {
            var fieldErrors = new List<FieldError>();
            result.Errors.ForEach(x => fieldErrors.Add(new FieldError(x.PropertyName, x.ErrorMessage)));

            return new UserPayload(input.UsernameOrEmail, fieldErrors.ToArray());
        }

        var user = new User() { PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password) };
        if (UserInputValidator.IsEmail(input.UsernameOrEmail))
            user.Email = input.UsernameOrEmail;
        else
            user.Username = input.UsernameOrEmail;

        await users.InsertOneAsync(user);

        return new UserPayload(input.UsernameOrEmail, null);
    }

    public async Task<UserPayload> LoginAsync(
      [Service] IMongoDatabase db,
      UserInput input)
    {
        string? email = UserInputValidator.IsEmail(
            input.UsernameOrEmail) ? input.UsernameOrEmail : null;
        User? user = email != null
            ? await db.GetCollection<User>("Users").Find(x => x.Email == input.UsernameOrEmail).FirstOrDefaultAsync()
            : await db.GetCollection<User>("Users").Find(x => x.Username == input.UsernameOrEmail).FirstOrDefaultAsync();
        if (user == null)
        {
            return new UserPayload(input.UsernameOrEmail, new FieldError[]
                {
                    new FieldError(nameof(input.UsernameOrEmail), Consts.INVALID_USERNAME)
                });
        }

        if (!BCrypt.Net.BCrypt.Verify(input.Password, user.PasswordHash))
        {
            return new UserPayload(input.UsernameOrEmail, new FieldError[]
                {
                    new FieldError(nameof(input.Password), Consts.INVALID_PASSWORD)
                });
        }

        return new UserPayload(input.UsernameOrEmail, null);
    }
}
