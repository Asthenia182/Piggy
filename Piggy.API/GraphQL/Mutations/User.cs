using MongoDB.Driver;
using Piggy.API.Utils;

public partial class Mutation
{
    public async Task<UserPayload> RegisterAsync(
      [Service] IMongoDatabase db, [Service] IHttpContextAccessor httpContextAccessor,
      UserInput input)
    {
        var users = db.GetCollection<User>(MongoDBUtils.GetCollectionName<User>());
        var user = await GetUserAsync(input.UsernameOrEmail, db);
        if (user != null)
        {
            return new UserPayload(UserUtils.GetUsernameOrEmail(user), new FieldError[]
                   {
                    new FieldError(nameof(input.UsernameOrEmail), ValidationErrors.USER_EXISTS)
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
        if (UserUtils.IsEmail(input.UsernameOrEmail))
            user.Email = input.UsernameOrEmail;
        else
            user.Username = input.UsernameOrEmail;

        await users.InsertOneAsync(user);

        httpContextAccessor.HttpContext?.Session.SetString(Consts.COOKIE_NAME, user.Id);

        return new UserPayload(input.UsernameOrEmail, null);
    }

    public async Task<UserPayload> LoginAsync(
      [Service] IMongoDatabase db, [Service] IHttpContextAccessor httpContextAccessor,
      UserInput input)
    {
        User? user = await GetUserAsync(input.UsernameOrEmail, db);
        if (user == null)
        {
            return new UserPayload(input.UsernameOrEmail, new FieldError[]
                {
                    new FieldError(nameof(input.UsernameOrEmail), ValidationErrors.INVALID_USERNAME)
                });
        }

        if (!BCrypt.Net.BCrypt.Verify(input.Password, user.PasswordHash))
        {
            return new UserPayload(input.UsernameOrEmail, new FieldError[]
                {
                    new FieldError(nameof(input.Password), ValidationErrors.INVALID_PASSWORD)
                });
        }

        httpContextAccessor.HttpContext?.Session.SetString(Consts.COOKIE_NAME, user.Id);

        return new UserPayload(input.UsernameOrEmail, null);
    }

    private async Task<User> GetUserAsync(string usernameOrEmail, IMongoDatabase db)
    {
        var users = db.GetCollection<User>(MongoDBUtils.GetCollectionName<User>());
        string? email = UserUtils.IsEmail(usernameOrEmail) ? usernameOrEmail : null;
        User? user = email != null
            ? await users.Find(x => x.Email == usernameOrEmail).FirstOrDefaultAsync()
            : await users.Find(x => x.Username == usernameOrEmail).FirstOrDefaultAsync();

        return user;
    }
}
