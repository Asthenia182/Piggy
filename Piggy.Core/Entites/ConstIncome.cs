using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Piggy.Core.Entites;
public class ConstIncome : IIncome
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
}

