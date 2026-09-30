using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web_453503_Avramenko.API.Data;
using Web_453503_Avramenko.API.UseCases;
using Web_453503_Avramenko.Domain.Entities;

namespace Web_453503_Avramenko.API;

public static class PetEndpoints
{
    public static void MapPetEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pets")
            .DisableAntiforgery()
            .RequireAuthorization("admin");

        // group.MapGet("/{species?}",
        //         async Task<Results<Ok<ResponseData<ListModel<Pet>>>, NotFound>> (IMediator mediator, string? species, int page=1) =>
        //         {
        //             var data = await mediator.Send(new GetListOfPets(species, page));
        //             return TypedResults.Ok(data);
        //         })
        // .WithName("GetAllPets");

        group.MapGet("/{id:guid}", async Task<Results<Ok<Pet>, NotFound>> (Guid id, AppDbContext db) =>
            {
                var pet = await db.Pets.Include(m => m.Species).FirstOrDefaultAsync(m => m.Id == id);
                return await db.Pets
                        .Include(p => p.Species)
                        .FirstOrDefaultAsync(model => model.Id == id)
                    is Pet model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
            })
            .WithName("GetPetById");

        
        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (
                [FromRoute]Guid id, 
                [FromForm] string pet,
                [FromForm] IFormFile? file,
                AppDbContext db,
                IMediator mediator) =>
            {
                var newPet = JsonSerializer.Deserialize<Pet>(pet);

                if (newPet is null)
                {
                    return TypedResults.BadRequest();
                }

                if (file is not null)
                {
                    var oldPet = await db.Pets.FirstOrDefaultAsync(m => m.Id == id);

                    if (oldPet is not null && oldPet.Image is not null)
                    {
                       await mediator.Send(new DeleteImage(oldPet.Image));
                    }
                    
                    newPet.Image = await mediator.Send(new SaveImage(file));
                }
            
                var affected = await db.Pets
                    .Where(model => model.Id == id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.Id, newPet.Id)
                        .SetProperty(m => m.Name, newPet.Name)
                        .SetProperty(m => m.Description, newPet.Description)
                        .SetProperty(m => m.Weight, newPet.Weight)
                        .SetProperty(m => m.Image, newPet.Image)
                        .SetProperty(m => m.ImageType, newPet.ImageType)
                        .SetProperty(m => m.SpeciesId, newPet.SpeciesId)
                        );
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePet");

        // group.MapPost("/", async (Pet pet, AppDbContext db) =>
        // {
        //     db.Pets.Add(pet);
        //     await db.SaveChangesAsync();
        //     return TypedResults.Created($"/api/Pet/{pet.Id}",pet);
        // })
        // .WithName("CreatePet");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (
                Guid id, 
                AppDbContext db,
                IMediator mediator) =>
        {
            var oldPet = await db.Pets.FirstOrDefaultAsync(m => m.Id == id);

            if (oldPet is not null && oldPet.Image is not null)
            {
                await mediator.Send(new DeleteImage(oldPet.Image));
            }
            
            var affected = await db.Pets
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePet");
    }
}
