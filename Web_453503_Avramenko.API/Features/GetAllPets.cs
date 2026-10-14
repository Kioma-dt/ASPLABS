using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Web_453503_Avramenko.API.Abstractions;
using Web_453503_Avramenko.API.Data;

namespace Web_453503_Avramenko.API.Features;

public static class GetAllPets
{
    public sealed class EndPoint
        : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/pets/{species?}", Handler)
                .DisableAntiforgery()
                .WithName("GetPets")
                .WithTags("Pets");
        }
    }
    
    public static async Task<IResult> Handler(
        AppDbContext db,
        HybridCache cache,
        [FromRoute] string? species,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 3)
    {
        var count = await cache.GetOrCreateAsync(
            $"pets_{species}_count",
            async token => await db.Pets
                .Include(p => p.Species)
                .CountAsync(p => species == null
                                 || p.Species!.NormalizedName.Equals(species),
                    token),
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(1),
                LocalCacheExpiration = TimeSpan.FromSeconds(30)
            });
        
        var totalPages = (int) Math.Ceiling((double)count / pageSize);
        
        var items = await cache.GetOrCreateAsync(
            $"pets_{species}_{page}_{pageSize}",
            async token => await db.Pets
                .AsQueryable()
                .Include(p => p.Species)
                .Where(p => species == null
                            || (p.Species.NormalizedName.Equals(species)))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token),
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(1),
                LocalCacheExpiration = TimeSpan.FromSeconds(30)
            });
        
        var data = new ListModel<Pet>()
        {
            Items = items,
            CurrentPage = page,
            TotalPages = totalPages
        };

        var result = ResponseData<ListModel<Pet>>.Success(data);

        return TypedResults.Ok(ResponseData<ListModel<Pet>>.Success(data));
    }
}