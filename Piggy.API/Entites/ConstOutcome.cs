
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonCollection("ConstOutcomes")]
public class ConstOutcome
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string? Name { get; set; }
    public double Amount { get; set; }
    public string? Currency { get; set; }
    public Category? Category { get; set; }
}

