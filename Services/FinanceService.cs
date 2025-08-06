using FinanceApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FinanceApi.Services;

public class FinanceService
{
    private readonly IMongoCollection<Account> _accounts;
    private readonly IMongoCollection<Balance> _balances;
    private readonly IMongoCollection<AccountGroup> _groups;

    public FinanceService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:Database"]);

        _accounts = database.GetCollection<Account>(config["MongoDB:AccountsCollection"]);
        _balances = database.GetCollection<Balance>(config["MongoDB:BalancesCollection"]);
        _groups = database.GetCollection<AccountGroup>(config["MongoDB:GroupsCollection"]);
    }

    // ------------------------------------------
    // ACCOUNT GROUPS
    // ------------------------------------------

    public async Task<List<AccountGroup>> GetGroupsAsync() =>
        await _groups.Find(_ => true).ToListAsync();

    public async Task<AccountGroup> CreateGroupAsync(AccountGroup group)
    {
        await _groups.InsertOneAsync(group);
        return group;
    }

    public async Task<AccountGroup?> UpdateGroupAsync(string id, AccountGroup updated)
    {
        var result = await _groups.ReplaceOneAsync(g => g.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteGroupAsync(string id)
    {
        var hasAccounts = await _accounts.Find(a => a.GroupId == id).AnyAsync();
        if (hasAccounts) return false;

        var result = await _groups.DeleteOneAsync(g => g.Id == id);
        return result.DeletedCount > 0;
    }

    // ------------------------------------------
    // ACCOUNTS
    // ------------------------------------------

    public async Task<List<Account>> GetAccountsAsync(string? groupId = null, string? nameContains = null)
    {
        var filterBuilder = Builders<Account>.Filter;
        var filters = new List<FilterDefinition<Account>>();

        if (!string.IsNullOrEmpty(groupId))
        {
            filters.Add(filterBuilder.Eq(a => a.GroupId, groupId));
        }

        if (!string.IsNullOrEmpty(nameContains))
        {
            filters.Add(filterBuilder.Regex(a => a.Name, new MongoDB.Bson.BsonRegularExpression(nameContains, "i")));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

        return await _accounts.Find(combinedFilter).ToListAsync();
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        await _accounts.InsertOneAsync(account);
        return account;
    }

    public async Task<Account?> UpdateAccountAsync(string id, Account updated)
    {
        var result = await _accounts.ReplaceOneAsync(a => a.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteAccountAsync(string id)
    {
        var result = await _accounts.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }

    // ------------------------------------------
    // BALANCES
    // ------------------------------------------

    public async Task<List<Balance>> GetBalancesByAccountAsync(string accountId) =>
        await _balances.Find(b => b.AccountId == accountId).ToListAsync();

    public async Task<Balance> CreateBalanceAsync(Balance balance)
    {
        await _balances.InsertOneAsync(balance);
        return balance;
    }

    public async Task<Balance?> UpdateBalanceAsync(string id, Balance updated)
    {
        var result = await _balances.ReplaceOneAsync(b => b.Id == id, updated);
        return result.MatchedCount > 0 ? updated : null;
    }

    public async Task<bool> DeleteBalanceAsync(string id)
    {
        var result = await _balances.DeleteOneAsync(b => b.Id == id);
        return result.DeletedCount > 0;
    }


    public async Task<List<AccountGroupWithAccountsDto>> GetGroupsWithAccountsAndAllBalancesAsync()
    {
        var groups = await _groups.Find(_ => true).ToListAsync();
        var accounts = await _accounts.Find(_ => true).ToListAsync();
        var accountIds = accounts.Select(a => a.Id).ToList();

        var balances = await _balances.Find(b => accountIds.Contains(b.AccountId)).ToListAsync();

        var result = groups.Select(group => new AccountGroupWithAccountsDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Accounts = accounts
                .Where(acc => acc.GroupId == group.Id)
                .Select(acc => new AccountWithAllBalancesDto
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Description = acc.Description,
                    Balances = balances
    .Where(b => b.AccountId == acc.Id)
    .Select(b => new BalanceDto
    {
        Year = b.Year,
        Month = b.Month,
        Amount = b.Amount
    })
    .OrderBy(b => b.Year)
    .ThenBy(b => b.Month)
    .ToList()
                })
                .ToList()
        }).ToList();

        return result;
    }
}

