using FinanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeBE.Controllers.BusinessFinance;

[ApiController]
[Route("api/[controller]")]
public class FinancialYearsController : ControllerBase
{
    private readonly BusinessFinanceService _service;

    public FinancialYearsController(BusinessFinanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFinancialYears()
    {
        var financialYears = await _service.GetFinancialYearsAsync();
        return Ok(financialYears);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFinancialYear(FinancialYear financialYear)
    {
        var created = await _service.CreateFinancialYearAsync(financialYear);
        return CreatedAtAction(nameof(GetFinancialYears), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinancialYear(string id, FinancialYear financialYear)
    {
        var updated = await _service.UpdateFinancialYearAsync(id, financialYear);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteFinancialYear(string id)
    //{
    //    var deleted = await _service.DeleteFinancialYearAsync(id);
    //    if (!deleted) return NotFound();
    //    return NoContent(); // 204
    //}
}
