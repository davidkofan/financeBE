using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Balance
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string AccountId { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
}
