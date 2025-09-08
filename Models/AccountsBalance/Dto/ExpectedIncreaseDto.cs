using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ExpectedIncreaseDto
{

    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
}
