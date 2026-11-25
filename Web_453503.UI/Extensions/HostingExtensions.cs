using Web_453503.UI.Services.PetService;
using Web_453503.UI.Services.SpeciesService;

namespace Web_453503.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISpeciesService, MemorySpeciesService>();
        builder.Services.AddScoped<IPetService, MemoryPetService>();
    }
}