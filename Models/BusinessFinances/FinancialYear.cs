using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class FinancialYear
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal AdditionalTax { get; set; }
    public decimal AdditionalHealthInsurance { get; set; }
    public decimal AdditionalSocialInsurance { get; set; }

}
