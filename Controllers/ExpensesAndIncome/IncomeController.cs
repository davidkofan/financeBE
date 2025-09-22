using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.AccountsBalance;

[ApiController]
[Route("api/[controller]")]
public class IncomeController : ControllerBase
{
    private readonly ExpensesAndIncomeService _service;

    public IncomeController(ExpensesAndIncomeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetIncomes([FromQuery] string? groupId = null, [FromQuery] string? name = null)
    {
        var incomes = await _service.GetIncomesAsync(groupId, name);
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncome(Income income)
    {
        var created = await _service.CreateIncomeAsync(income);
        return CreatedAtAction(nameof(GetIncomes), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(string id, Income income)
    {
        var updated = await _service.UpdateIncomeAsync(id, income);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(string id)
    {
        var deleted = await _service.DeleteIncomeAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204
    }
}
