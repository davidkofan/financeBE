using FinanceApi.Services;
using financeBE.Models.AccountsBalance;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExpenseGroupsController : ControllerBase
{
    private readonly ExpensesAndIncomeService _service;

    public ExpenseGroupsController(ExpensesAndIncomeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _service.GetGroupsAsync();
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup(ExpenseGroup group)
    {
        var created = await _service.CreateGroupAsync(group);
        return CreatedAtAction(nameof(GetGroups), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(string id, ExpenseGroup group)
    {
        var updated = await _service.UpdateGroupAsync(id, group);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(string id)
    {
        var deleted = await _service.DeleteGroupAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204
    }

    //[HttpGet("overview/full")]
    //public async Task<IActionResult> GetOverviewWithAllBalances()
    //{
    //    var data = await _service.GetGroupsWithAccountsAndAllBalancesAsync();
    //    return Ok(data);
    //}

}
