using financeBE.Models.AccountsBalance;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FinanceApi.Services;

public class BusinessFinanceService
{
    private readonly IMongoCollection<FinancialYear> _financialYears;
    private readonly IMongoCollection<MonthlyBalance> _monthlyBalances;

    public BusinessFinanceService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:Database"]);

        _financialYears = database.GetCollection<FinancialYear>(config["MongoDB:FinancialYearsCollection"]);
        _monthlyBalances = database.GetCollection<MonthlyBalance>(config["MongoDB:MonthlyBalancesCollection"]);
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

    // ------------------------------------------
    // BALANCES
    // ------------------------------------------

    public async Task<List<MonthlyBalance>> GetMonthlyBalancesByFinancialYearAsync(string financialYearId) =>
        await _monthlyBalances.Find(b => b.FinancialYearId == financialYearId).ToListAsync();

    public async Task<MonthlyBalance> CreateMonthlyBalanceAsync(MonthlyBalance monthlyBalance)
    {
        await _monthlyBalances.InsertOneAsync(monthlyBalance);
        return monthlyBalance;
    }

    public async Task<MonthlyBalance?> UpdateMonthlyBalanceAsync(string id, MonthlyBalance updated)
    {
        var result = await _monthlyBalances.ReplaceOneAsync(b => b.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteMonthlyBalanceAsync(string id)
    {
        var result = await _monthlyBalances.DeleteOneAsync(b => b.Id == id);
        return result.DeletedCount > 0;
    }
}

