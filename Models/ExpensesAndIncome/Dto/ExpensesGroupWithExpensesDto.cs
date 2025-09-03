namespace financeBE.DTOs;

public class ExpensesGroupWithExpensesDto
{
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Expense> Expenses { get; set; }
}
