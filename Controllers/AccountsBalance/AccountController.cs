using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.AccountsBalance;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountsBalanceService _service;

    public AccountsController(AccountsBalanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] string? groupId = null, [FromQuery] string? name = null)
    {
        var accounts = await _service.GetAccountsAsync(groupId, name);
        return Ok(accounts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(Account account)
    {
        var created = await _service.CreateAccountAsync(account);
        return CreatedAtAction(nameof(GetAccounts), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(string id, Account account)
    {
        var updated = await _service.UpdateAccountAsync(id, account);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(string id)
    {
        var deleted = await _service.DeleteAccountAsync(id);
        if (!deleted) return NotFound();
        return NoContent(); // 204
    }
}
