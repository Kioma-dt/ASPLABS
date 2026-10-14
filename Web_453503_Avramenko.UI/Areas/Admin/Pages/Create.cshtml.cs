using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_453503_Avramenko.Domain.Entities;
using Web_453503_Avramenko.UI.Services.PetService;

namespace Web_453503_Avramenko.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly ISpeciesService _speciesService;

        public CreateModel(
            IPetService petService,
            ISpeciesService speciesService)
        {
            _petService = petService;
            _speciesService = speciesService;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["SpeciesId"] = new SelectList(
                (await _speciesService.GetSpeciesListAsync()).Data.ToList(), 
                "Id", 
                "Name");
            return Page();
        }

        [BindProperty]
        public Pet Pet { get; set; } = default!;
        
        [BindProperty]
        public IFormFile? Image { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _petService.CreatePetAsync(Pet, Image);

            return RedirectToPage("./Index");
        }
    }
}
