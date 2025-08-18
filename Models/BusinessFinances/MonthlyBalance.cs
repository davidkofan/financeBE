using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MonthlyBalance
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string FinancialYearId { get; set; }

    public int Month { get; set; }
    public string? Description { get; set; }
    public decimal Income { get; set; }
    public decimal Tax { get; set; }
    public decimal HealthInsurance { get; set; }
    public decimal SocialInsurance { get; set; }

}
