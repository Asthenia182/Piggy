public class MongoDBUtils
{
    public static string GetCollectionName<T>()
    {
        return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).First()
            as BsonCollectionAttribute).CollectionName;
    }
}

