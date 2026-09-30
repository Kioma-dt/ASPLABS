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
    public class IndexModel : PageModel
    {
        private readonly IPetService _petService;

        public IndexModel(IPetService petService)
        {
            _petService = petService;
        }

        public ListModel<Pet> Pet { get; set; } = new();

        public async Task OnGetAsync(int pageNumber = 1)
        {
            pageNumber = Math.Max(pageNumber, 1);
            Pet = (await _petService.GetPetListAsync(null, pageNumber)).Data
                  ?? new ListModel<Pet>();
        }
    }
}
