using MongoDB.Driver;

public partial class Mutation
{
    public async Task<UserPayload> RegisterAsync(
      [Service] IMongoDatabase db,
      UserInput input)
    {
        var users = db.GetCollection<User>(MongoDBUtils.GetCollectionName<User>());
        var user = await GetUserAsync(input.UsernameOrEmail, db);
        if (user != null)
        {
            return new UserPayload(input.UsernameOrEmail, new FieldError[]
                   {
                    new FieldError(nameof(input.UsernameOrEmail), Consts.USER_EXISTS)
                   });
        }

        var validationResult = new UserInputValidator().Validate(input);
        if (!validationResult.IsValid)
        {
            var fieldErrors = new List<FieldError>();
            validationResult.Errors.ForEach(x => fieldErrors.Add(
                new FieldError(x.PropertyName, x.ErrorMessage))
            );
            return new UserPayload(input.UsernameOrEmail, fieldErrors.ToArray());
        }

        user = new User() { PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password) };
        if (UserInputValidator.IsEmail(input.UsernameOrEmail))
            user.Email = input.UsernameOrEmail;
        else
            user.Username = input.UsernameOrEmail;

        try
        {
            await users.InsertOneAsync(user);
        }
        catch (Exception ex)
        {

            throw;
        }

        return new UserPayload(input.UsernameOrEmail, null);
    }

    public async Task<UserPayload> LoginAsync(
      [Service] IMongoDatabase db,
      UserInput input)
    {
        User? user = await GetUserAsync(input.UsernameOrEmail, db);
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

    private async Task<User> GetUserAsync(string usernameOrEmail, IMongoDatabase db)
    {
        var users = db.GetCollection<User>(MongoDBUtils.GetCollectionName<User>());
        string? email = UserInputValidator.IsEmail(usernameOrEmail) ? usernameOrEmail : null;
        User? user = email != null
            ? await users.Find(x => x.Email == usernameOrEmail).FirstOrDefaultAsync()
            : await users.Find(x => x.Username == usernameOrEmail).FirstOrDefaultAsync();

        return user;
    }
}
