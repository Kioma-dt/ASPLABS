using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Web_453503_Avramenko.API.Data;
using Web_453503_Avramenko.API.UseCases;
using Web_453503_Avramenko.Domain.Entities;
namespace Web_453503_Avramenko.API;

public static class PetEndpoints
{
    public static void MapPetEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Pet");

        group.MapGet("/{species?}",
                async Task<Results<Ok<ResponseData<ListModel<Pet>>>, NotFound>> (IMediator mediator, string? species, int page=1) =>
                {
                    var data = await mediator.Send(new GetListOfPets(species, page));
                    return TypedResults.Ok(data);
                })
        .WithName("GetAllPets");

        group.MapGet("/{id:guid}", async Task<Results<Ok<Pet>, NotFound>> (Guid id, AppDbContext db) =>
        {
            return await db.Pets.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Pet model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPetById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Pet pet, AppDbContext db) =>
        {
            var affected = await db.Pets
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, pet.Id)
                    .SetProperty(m => m.Name, pet.Name)
                    .SetProperty(m => m.Description, pet.Description)
                    .SetProperty(m => m.Weight, pet.Weight)
                    .SetProperty(m => m.Image, pet.Image)
                    .SetProperty(m => m.ImageType, pet.ImageType)
                    .SetProperty(m => m.SpeciesId, pet.SpeciesId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePet");

        group.MapPost("/", async (Pet pet, AppDbContext db) =>
        {
            db.Pets.Add(pet);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Pet/{pet.Id}",pet);
        })
        .WithName("CreatePet");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, AppDbContext db) =>
        {
            var affected = await db.Pets
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePet");
    }
}
