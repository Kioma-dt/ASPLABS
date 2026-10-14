using Web_453503_Avramenko.UI.Services.PetService;
using Web_453503_Avramenko.UI.Services.SpeciesService;

namespace Web_453503_Avramenko.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISpeciesService, MemorySpeciesService>();
        builder.Services.AddScoped<IPetService, MemoryPetService>();
    }
}