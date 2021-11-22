using MongoDB.Driver;
using Piggy.API.Utils;

public partial class Query
{
    public UserPayload IsLogged([Service] IMongoDatabase db,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.Session.GetString(Consts.COOKIE_NAME);
        if (string.IsNullOrEmpty(userId))
        {
            return new UserPayload("", null);
        }
        var users = db.GetCollection<User>(MongoDBUtils.GetCollectionName<User>());
        var user = users.Find(x => x.Id == userId).First();

        return new UserPayload(UserUtils.GetUsernameOrEmail(user), null);
    }
}