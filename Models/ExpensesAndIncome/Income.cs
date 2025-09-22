using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Income
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? GroupId { get; set; } // môže byť null, ak príjem nie je priradený

}
