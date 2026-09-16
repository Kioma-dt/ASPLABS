using Microsoft.AspNetCore.Mvc;
using Web_453503_Avramenko.UI.Services.PetService;

namespace Web_453503_Avramenko.UI.Controllers;

public class PetController
    : Controller
{
    readonly ISpeciesService _speciesService;
    readonly IPetService _petService;

    public PetController(
        ISpeciesService speciesService,
        IPetService petService)
    {
        _speciesService = speciesService;
        _petService = petService;
    }

    public async Task<IActionResult> Index(string? species, int page = 1)
    {
        var speciesList = (await _speciesService.GetSpeciesListAsync()).Data
            ??  new List<Species>();
        var currentSpecies = speciesList.FirstOrDefault(s => s.NormalizedName.Equals(species));
        ViewData["currentSpeciesName"] = currentSpecies?.Name ?? "All";
        ViewData["speciesList"] = speciesList;
        
        var petResponse = await _petService.GetPetListAsync(species, page);

        if (!petResponse.IsSuccessfully)
        {
            return NotFound(petResponse.ErrorMessage);
        }

        return View(petResponse?.Data);
    }
}