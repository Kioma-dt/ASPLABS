namespace Web_453503_Avramenko.UI.Services.SpeciesService;

public interface ISpeciesService
{
    Task<ResponseData<List<Species>>> GetSpeciesListAsync();
}