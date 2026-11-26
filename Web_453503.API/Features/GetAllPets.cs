using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Web_453503.API.Abstractions;
using Web_453503.API.Data;

namespace Web_453503.API.Features;

public static class GetAllPets
{
    private static readonly int MaxPageSize = 20;
    
    public sealed class EndPoint
        : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/pets/{species?}", Handler)
                .DisableAntiforgery()
                .RequireAuthorization()
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
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        
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

        if (page < 1 || page > Math.Max(1, totalPages))
        {
            return TypedResults.Ok(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = false,
                ErrorMessage = "Invalid Page Number"
            });
        }
        
        var items = await cache.GetOrCreateAsync(
            $"pets_{species}_{page}_{pageSize}",
            async token => await db.Pets
                .AsQueryable()
                .Include(p => p.Species)
                .Where(p => species == null
                            || (p.Species.NormalizedName.Equals(species)))
                .OrderBy(p => p.Id)
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