using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonCollection("TempIncomes")]
public class TempIncome : IIncome
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
    public DateTime DateTime { get; set; }

}