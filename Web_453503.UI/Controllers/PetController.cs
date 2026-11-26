using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_453503.UI.Services.PetService;
using Web_453503.UI.Services.SpeciesService;

namespace Web_453503.UI.Controllers;

[Route("catalog")]
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

    [Authorize]
    [Route("{species?}")]
    public async Task<IActionResult> Index(string? species, int page = 1)
    {
        var speciesResponse = await _speciesService.GetSpeciesListAsync();

        if (!speciesResponse.IsSuccessfully)
        {
            return NotFound(speciesResponse.ErrorMessage);
        }
        
        var speciesList = speciesResponse.Data ??  new List<Species>();
        var currentSpecies = speciesList.FirstOrDefault(s => s.NormalizedName.Equals(species));
        ViewData["currentSpeciesName"] = currentSpecies?.Name ?? "All";
        ViewData["speciesList"] = speciesList;
        
        var petResponse = await _petService.GetPetListAsync(species, page);

        if (!petResponse.IsSuccessfully)
        {
            return NotFound(petResponse.ErrorMessage);
        }

        if (Request.IsAjaxRequest())
        {
            return PartialView("_PetListPartial", petResponse.Data);
        }

        return View(petResponse?.Data);
    }
}