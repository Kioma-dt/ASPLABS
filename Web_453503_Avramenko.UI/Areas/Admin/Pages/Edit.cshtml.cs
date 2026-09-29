using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_453503_Avramenko.Domain.Entities;
using Web_453503_Avramenko.UI.Services.PetService;

namespace Web_453503_Avramenko.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly ISpeciesService _speciesService;

        public EditModel(
            IPetService petService,
            ISpeciesService speciesService)
        {
            _petService = petService;
            _speciesService = speciesService;
        }

        [BindProperty]
        public Pet Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = (await _petService.GetPetByIdAsync(id.Value)).Data;
            if (pet == null)
            {
                return NotFound();
            }
            Pet = pet;
            ViewData["SpeciesId"] = new SelectList(
                (await _speciesService.GetSpeciesListAsync()).Data.ToList(), 
                "Id", 
                "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var pet = (await _petService.GetPetByIdAsync(Pet.Id)).Data;

            if (pet is not null)
            {
                await _petService.UpdatePetAsync(Pet.Id, Pet, null);
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }

        // private bool PetExists(Guid id)
        // {
        //     return _context.Pets.Any(e => e.Id == id);
        // }
    }
}
