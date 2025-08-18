public class AccountGroupWithAccountsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<AccountWithAllBalancesDto> Accounts { get; set; }
}