using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace financeBE.Models.AccountsBalance;

public class AccountGroup
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
