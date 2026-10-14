namespace Web_453503_Avramenko.Domain.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Weight { get; set; }
    public string? Image { get; set; }
    public string? ImageType { get; set; }
    
    public Guid? SpeciesId { get; set; }
    public Species? Species { get; set; }
}