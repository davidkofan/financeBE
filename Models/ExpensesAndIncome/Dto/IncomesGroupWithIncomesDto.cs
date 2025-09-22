namespace financeBE.DTOs;

public class IncomesGroupWithIncomesDto
{
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Income> Incomes { get; set; }
}
