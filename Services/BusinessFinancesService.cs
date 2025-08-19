using financeBE.DTOs;
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

    public async Task<bool> DeleteFinancialYearAsync(string id)
    {
        var hasBalance = await _monthlyBalances.Find(a => a.FinancialYearId == id).AnyAsync();
        if (hasBalance) return false;

        var result = await _financialYears.DeleteOneAsync(g => g.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<FinancialYearWithBalancesDto>> GetFinancialYearsWithAllBalancesAsync()
    {
        var financialYears = await _financialYears.Find(_ => true).ToListAsync();
        var monthlyBalances = await _monthlyBalances.Find(_ => true).ToListAsync();

        var result = financialYears
        .Select(fy => new FinancialYearWithBalancesDto
        {
            FinancialYear = fy,
            MonthlyBalances = monthlyBalances
                .Where(mb => mb.FinancialYearId == fy.Id)
                .ToList()
        })
        .ToList();

        return result;
    }

    // ------------------------------------------
    // MONTHLY BALANCES
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

