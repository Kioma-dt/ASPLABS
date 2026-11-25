namespace Web_453503.UI.Services.SpeciesService;

public interface ISpeciesService
{
    Task<ResponseData<List<Species>>> GetSpeciesListAsync();
}