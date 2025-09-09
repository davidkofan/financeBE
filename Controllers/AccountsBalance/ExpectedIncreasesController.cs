using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.AccountsBalance;

[ApiController]
[Route("api/[controller]")]
public class ExpectedIncreasesController : ControllerBase
{
    private readonly AccountsBalanceService _service;

    public ExpectedIncreasesController(AccountsBalanceService service)
    {
        _service = service;
    }

    [HttpGet("{accountGroupId}")]
    public async Task<IActionResult> GetExpectedIncreases(string accountGroupId)
    {
        var balances = await _service.GetExpectedIncreasesByAccountGroupAsync(accountGroupId);
        return Ok(balances);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpectedIncrease(ExpectedIncrease expectedIncrease)
    {
        var created = await _service.CreateExpectedIncreaseAsync(expectedIncrease);
        return CreatedAtAction(nameof(GetExpectedIncreases), new { accountGroupId = expectedIncrease.GroupId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpectedIncrease(string id, ExpectedIncrease expectedIncrease)
    {
        var updated = await _service.UpdateExpectedIncreaseAsync(id, expectedIncrease);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpectedIncrease(string id)
    {
        var deleted = await _service.DeleteExpectedIncreaseAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204 No Content
    }
}
