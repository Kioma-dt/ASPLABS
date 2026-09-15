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

    public async Task<IActionResult> Index()
    {
        var petResponse = await _petService.GetPetListAsync(null);

        if (!petResponse.IsSuccessfully)
        {
            return NotFound(petResponse.ErrorMessage);
        }

        return View(petResponse.Data.Items);
    }
}