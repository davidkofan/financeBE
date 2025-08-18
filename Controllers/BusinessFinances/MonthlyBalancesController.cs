using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.BusinessFinance;

[ApiController]
[Route("api/[controller]")]
public class MonthlyBalancesController : ControllerBase
{
    private readonly BusinessFinanceService _service;

    public MonthlyBalancesController(BusinessFinanceService service)
    {
        _service = service;
    }

    [HttpGet("{financialYearId}")]
    public async Task<IActionResult> GetMonthlyBalances(string financialYearId)
    {
        var monthlyBalances = await _service.GetMonthlyBalancesByFinancialYearAsync(financialYearId);
        return Ok(monthlyBalances);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMonthlyBalance(MonthlyBalance monthlyBalance)
    {
        var created = await _service.CreateMonthlyBalanceAsync(monthlyBalance);
        return CreatedAtAction(nameof(GetMonthlyBalances), new { financialYearId = monthlyBalance.FinancialYearId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMonthlyBalance(string id, MonthlyBalance monthlyBalance)
    {
        var updated = await _service.UpdateMonthlyBalanceAsync(id, monthlyBalance);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMonthlyBalance(string id)
    {
        var deleted = await _service.DeleteMonthlyBalanceAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204 No Content
    }
}