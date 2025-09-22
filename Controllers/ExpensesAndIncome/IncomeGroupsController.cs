using FinanceApi.Services;
using financeBE.Models.AccountsBalance;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IncomeGroupsController : ControllerBase
{
    private readonly ExpensesAndIncomeService _service;

    public IncomeGroupsController(ExpensesAndIncomeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _service.GetIncomeGroupsAsync();
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup(IncomeGroup group)
    {
        var created = await _service.CreateIncomeGroupAsync(group);
        return CreatedAtAction(nameof(GetGroups), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(string id, IncomeGroup group)
    {
        var updated = await _service.UpdateIncomeGroupAsync(id, group);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(string id)
    {
        var deleted = await _service.DeleteIncomeGroupAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204
    }

    [HttpGet("overview/full")]
    public async Task<IActionResult> GetOverviewWithAllIncomes()
    {
        var data = await _service.GetIncomeGroupsWithIncomesAsync();
        return Ok(data);
    }
}
