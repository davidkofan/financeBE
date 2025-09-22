using financeBE.DTOs;
using financeBE.Models.AccountsBalance;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FinanceApi.Services;

public class ExpensesAndIncomeService
{
    private readonly IMongoCollection<Expense> _expenses;
    private readonly IMongoCollection<ExpenseGroup> _expensesGroups;
    private readonly IMongoCollection<Income> _incomes;
    private readonly IMongoCollection<IncomeGroup> _incomeGroups;

    public ExpensesAndIncomeService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:Database"]);

        _expenses = database.GetCollection<Expense>(config["MongoDB:ExpensesCollection"]);
        _expensesGroups = database.GetCollection<ExpenseGroup>(config["MongoDB:ExpensesGroupsCollection"]);

        _incomes = database.GetCollection<Income>(config["MongoDB:IncomesCollection"]);
        _incomeGroups = database.GetCollection<IncomeGroup>(config["MongoDB:IncomeGroupsCollection"]);
    }

    // ------------------------------------------
    // EXPENSE GROUPS
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

    public async Task<List<ExpensesGroupWithExpensesDto>> GetExpensesGroupsWithExpensesAsync()
    {
        var expensesGroups = await _expensesGroups.Find(_ => true).ToListAsync();
        var expenses = await _expenses.Find(_ => true).ToListAsync();

        var result = expensesGroups
            .Select(g => new ExpensesGroupWithExpensesDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Expenses = expenses.Where(e => e.GroupId == g.Id).ToList()
            })
            .ToList();

        return result;
    }

    // ------------------------------------------
    // EXPENSES
    // ------------------------------------------

    public async Task<List<Expense>> GetExpensesAsync(string? groupId = null, string? nameContains = null)
    {
        var filterBuilder = Builders<Expense>.Filter;
        var filters = new List<FilterDefinition<Expense>>();

        if (!string.IsNullOrEmpty(groupId))
            filters.Add(filterBuilder.Eq(a => a.GroupId, groupId));

        if (!string.IsNullOrEmpty(nameContains))
            filters.Add(filterBuilder.Regex(a => a.Name, new MongoDB.Bson.BsonRegularExpression(nameContains, "i")));

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

    // ------------------------------------------
    // INCOME GROUPS
    // ------------------------------------------

    public async Task<List<IncomeGroup>> GetIncomeGroupsAsync() =>
        await _incomeGroups.Find(_ => true).ToListAsync();

    public async Task<IncomeGroup> CreateIncomeGroupAsync(IncomeGroup group)
    {
        await _incomeGroups.InsertOneAsync(group);
        return group;
    }

    public async Task<IncomeGroup?> UpdateIncomeGroupAsync(string id, IncomeGroup updated)
    {
        var result = await _incomeGroups.ReplaceOneAsync(g => g.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteIncomeGroupAsync(string id)
    {
        var hasIncomes = await _incomes.Find(a => a.GroupId == id).AnyAsync();
        if (hasIncomes) return false;

        var result = await _incomeGroups.DeleteOneAsync(g => g.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<IncomesGroupWithIncomesDto>> GetIncomeGroupsWithIncomesAsync()
    {
        var incomeGroups = await _incomeGroups.Find(_ => true).ToListAsync();
        var incomes = await _incomes.Find(_ => true).ToListAsync();

        var result = incomeGroups
            .Select(g => new IncomesGroupWithIncomesDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Incomes = incomes.Where(i => i.GroupId == g.Id).ToList()
            })
            .ToList();

        return result;
    }

    // ------------------------------------------
    // INCOMES
    // ------------------------------------------

    public async Task<List<Income>> GetIncomesAsync(string? groupId = null, string? nameContains = null)
    {
        var filterBuilder = Builders<Income>.Filter;
        var filters = new List<FilterDefinition<Income>>();

        if (!string.IsNullOrEmpty(groupId))
            filters.Add(filterBuilder.Eq(a => a.GroupId, groupId));

        if (!string.IsNullOrEmpty(nameContains))
            filters.Add(filterBuilder.Regex(a => a.Name, new MongoDB.Bson.BsonRegularExpression(nameContains, "i")));

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;
        return await _incomes.Find(combinedFilter).ToListAsync();
    }

    public async Task<Income> CreateIncomeAsync(Income income)
    {
        await _incomes.InsertOneAsync(income);
        return income;
    }

    public async Task<Income?> UpdateIncomeAsync(string id, Income updated)
    {
        var result = await _incomes.ReplaceOneAsync(a => a.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteIncomeAsync(string id)
    {
        var result = await _incomes.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }
}
