using financeBE.Models.AccountsBalance;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FinanceApi.Services;

public class ExpensesAndIncomeService
{
    private readonly IMongoCollection<Expense> _expenses;
    private readonly IMongoCollection<ExpenseGroup> _expensesGroups;

    public ExpensesAndIncomeService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:Database"]);

        _expenses = database.GetCollection<Expense>(config["MongoDB:ExpensesCollection"]);
        _expensesGroups = database.GetCollection<ExpenseGroup>(config["MongoDB:ExpensesGroupsCollection"]);
    }

    // ------------------------------------------
    // EXPENSES GROUPS
    // ------------------------------------------

    public async Task<List<ExpenseGroup>> GetGroupsAsync() =>
        await _expensesGroups.Find(_ => true).ToListAsync();

    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group)
    {
        await _expensesGroups.InsertOneAsync(group);
        return group;
    }

    public async Task<ExpenseGroup?> UpdateGroupAsync(string id, ExpenseGroup updated)
    {
        var result = await _expensesGroups.ReplaceOneAsync(g => g.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteGroupAsync(string id)
    {
        var hasExpenses = await _expenses.Find(a => a.GroupId == id).AnyAsync();
        if (hasExpenses) return false;

        var result = await _expensesGroups.DeleteOneAsync(g => g.Id == id);
        return result.DeletedCount > 0;
    }

    //public async Task<List<AccountGroupWithAccountsDto>> GetGroupsWithAccountsAndAllBalancesAsync()
    //{
    //    var groups = await _groups.Find(_ => true).ToListAsync();
    //    var accounts = await _accounts.Find(_ => true).ToListAsync();
    //    var accountIds = accounts.Select(a => a.Id).ToList();

    //    var balances = await _balances.Find(b => accountIds.Contains(b.AccountId)).ToListAsync();

    //    var result = groups.Select(group => new AccountGroupWithAccountsDto
    //    {
    //        Id = group.Id,
    //        Name = group.Name,
    //        Description = group.Description,
    //        Accounts = accounts
    //            .Where(acc => acc.GroupId == group.Id)
    //            .Select(acc => new AccountWithAllBalancesDto
    //            {
    //                Id = acc.Id,
    //                Name = acc.Name,
    //                Description = acc.Description,
    //                Balances = balances
    //                    .Where(b => b.AccountId == acc.Id)
    //                    .Select(b => new BalanceDto
    //                    {
    //                        Year = b.Year,
    //                        Month = b.Month,
    //                        Amount = b.Amount
    //                    })
    //                    .OrderBy(b => b.Year)
    //                    .ThenBy(b => b.Month)
    //                    .ToList()
    //            })
    //            .ToList()
    //    }).ToList();

    //    return result;
    //}

    // ------------------------------------------
    // EXPENSES
    // ------------------------------------------

    public async Task<List<Expense>> GetExpensesAsync(string? groupId = null, string? nameContains = null)
    {
        var filterBuilder = Builders<Expense>.Filter;
        var filters = new List<FilterDefinition<Expense>>();

        if (!string.IsNullOrEmpty(groupId))
        {
            filters.Add(filterBuilder.Eq(a => a.GroupId, groupId));
        }

        if (!string.IsNullOrEmpty(nameContains))
        {
            filters.Add(filterBuilder.Regex(a => a.Name, new MongoDB.Bson.BsonRegularExpression(nameContains, "i")));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

        return await _expenses.Find(combinedFilter).ToListAsync();
    }

    public async Task<Expense> CreateExpenseAssync(Expense expense)
    {
        await _expenses.InsertOneAsync(expense);
        return expense;
    }

    public async Task<Expense?> UpdateExpensestAsync(string id, Expense updated)
    {
        var result = await _expenses.ReplaceOneAsync(a => a.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteExpensesAsync(string id)
    {
        var result = await _expenses.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }
}

