using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Web_453503_Avramenko.API.Data;
using Web_453503_Avramenko.Domain.Entities;
namespace Web_453503_Avramenko.API;

public static class SpeciesEndpoints
{
    public static void MapSpeciesEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Species");

        group.MapGet("/",
                async Task<Results<Ok<ResponseData<List<Species>>>, NotFound>> (AppDbContext db) =>
        {
            return TypedResults.Ok(ResponseData<List<Species>>.Success(await db.Species.ToListAsync()));
        })
        .WithName("GetAllSpecies");

        group.MapGet("/{id}", async Task<Results<Ok<Species>, NotFound>> (Guid id, AppDbContext db) =>
        {
            return await db.Species.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Species model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetSpeciesById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Species species, AppDbContext db) =>
        {
            var affected = await db.Species
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, species.Id)
                    .SetProperty(m => m.Name, species.Name)
                    .SetProperty(m => m.NormalizedName, species.NormalizedName)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateSpecies");

        group.MapPost("/", async (Species species, AppDbContext db) =>
        {
            db.Species.Add(species);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Species/{species.Id}",species);
        })
        .WithName("CreateSpecies");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, AppDbContext db) =>
        {
            var affected = await db.Species
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteSpecies");
    }
}
