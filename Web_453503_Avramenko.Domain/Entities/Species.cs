namespace Web_453503_Avramenko.Domain.Entities;

public class Species
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
}