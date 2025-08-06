public class AccountWithAllBalancesDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BalanceDto> Balances { get; set; }
}