using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Account
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string? GroupId { get; set; } // môže byť null, ak účet nie je priradený

}
