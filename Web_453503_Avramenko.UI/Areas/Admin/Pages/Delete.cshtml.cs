using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_453503_Avramenko.Domain.Entities;
using Web_453503_Avramenko.UI.Services.PetService;

namespace Web_453503_Avramenko.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IPetService _petService;

        public DeleteModel(IPetService petService)
        {
            _petService = petService;
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

            if (pet is not null)
            {
                Pet = pet;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = (await _petService.GetPetByIdAsync(id.Value)).Data;
            if (pet != null)
            {
                Pet = pet;
                await _petService.DeletePetAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
