namespace financeBE.DTOs;

public class FinancialYearWithBalancesDto
{
    public FinancialYear FinancialYear { get; set; }
    public List<MonthlyBalance> MonthlyBalances { get; set; }
}
