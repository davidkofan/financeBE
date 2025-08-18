using financeBE.Models.AccountsBalance;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FinanceApi.Services;

public class BusinessFinanceService
{
    private readonly IMongoCollection<FinancialYear> _financialYears;

    public BusinessFinanceService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:Database"]);

        _financialYears = database.GetCollection<FinancialYear>(config["MongoDB:FinancialYearsCollection"]);
    }

    // ------------------------------------------
    // FINANCIAL YEARS
    // ------------------------------------------

    public async Task<List<FinancialYear>> GetFinancialYearsAsync() =>
        await _financialYears.Find(_ => true).ToListAsync();

    public async Task<FinancialYear> CreateFinancialYearAsync(FinancialYear financialYear)
    {
        await _financialYears.InsertOneAsync(financialYear);
        return financialYear;
    }

    public async Task<FinancialYear?> UpdateFinancialYearAsync(string id, FinancialYear updated)
    {
        var result = await _financialYears.ReplaceOneAsync(g => g.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }
}

