namespace Web_453503_Avramenko.UI.Services.SpeciesService;

public class MemorySpeciesService
    : ISpeciesService
{
    public Task<ResponseData<List<Species>>> GetSpeciesListAsync()
    {
        var items = new List<Species>()
        {
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Cat",
                NormalizedName = "cat"
            },
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Name = "Dog",
                NormalizedName = "dog"
            },
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                Name = "Pigeon",
                NormalizedName = "pigeon"
            },
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Name = "Hamster",
                NormalizedName = "hamster"
            },
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000005"),
                Name = "Guinea Pig",
                NormalizedName = "guinea-pig"
            },
            new Species()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Name = "Spider",
                NormalizedName = "spider"
            },
        };

        var result = ResponseData<List<Species>>.Success(items);
        
        return Task.FromResult(result);
    }
}