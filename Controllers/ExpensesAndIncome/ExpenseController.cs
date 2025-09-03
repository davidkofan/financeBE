using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.AccountsBalance;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly ExpensesAndIncomeService _service;

    public ExpenseController(ExpensesAndIncomeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses([FromQuery] string? groupId = null, [FromQuery] string? name = null)
    {
        var expenses = await _service.GetExpensesAsync(groupId, name);
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpenses(Expense expense)
    {
        var created = await _service.CreateExpenseAssync(expense);
        return CreatedAtAction(nameof(GetExpenses), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpenses(string id, Expense expense)
    {
        var updated = await _service.UpdateExpensestAsync(id, expense);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpenses(string id)
    {
        var deleted = await _service.DeleteExpensesAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204
    }
}
