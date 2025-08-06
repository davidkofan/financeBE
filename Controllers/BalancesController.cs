using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BalancesController : ControllerBase
{
    private readonly FinanceService _service;

    public BalancesController(FinanceService service)
    {
        _service = service;
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetBalances(string accountId)
    {
        var balances = await _service.GetBalancesByAccountAsync(accountId);
        return Ok(balances);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBalance(Balance balance)
    {
        var created = await _service.CreateBalanceAsync(balance);
        return CreatedAtAction(nameof(GetBalances), new { accountId = balance.AccountId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBalance(string id, Balance balance)
    {
        var updated = await _service.UpdateBalanceAsync(id, balance);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBalance(string id)
    {
        var deleted = await _service.DeleteBalanceAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204 No Content
    }
}
