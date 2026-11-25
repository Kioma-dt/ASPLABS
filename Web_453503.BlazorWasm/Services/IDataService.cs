using Web_453503.Domain.Entities;

namespace Web_453503.BlazorWasm.Services;

public interface IDataService
{
    event Action DataLoaded;
    
    List<Species> Species { get; set; }
    List<Pet> Pets { get; set; }
    
    bool Success { get; set; }
    string ErrorMessage { get; set; }
    int TotalPages { get; set; }
    int CurrentPage { get; set; }

    Species? SelectedSpecies { get; set; }
    
    public Task GetPetsListAsync(int pageNo = 1);

    public Task GetSpeciesListAsync();
}