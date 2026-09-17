using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web_453503_Avramenko.API.Abstractions;
using Web_453503_Avramenko.API.Data;

namespace Web_453503_Avramenko.API.Features;

public static class AddNewPet
{
    public sealed record RequestDto(
        string Name,
        string Description,
        double Weight,
        Guid SpeciesId);

    public class RequestValidator
        : AbstractValidator<RequestDto>
    {
        public RequestValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(r => r.Description)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(r => r.Weight)
                .GreaterThan(0);
        }
    }

    public sealed class EndPoint
        : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/pets", Handler)
                .DisableAntiforgery()
                .WithName("CreatePet")
                .WithTags("Pets");
        }
    }

    public static async Task<IResult> Handler(
        [FromBody] RequestDto request,
        AppDbContext db,
        IValidator<RequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults
                .BadRequest(error: validationResult
                    .Errors
                    .Select(e => e.ErrorMessage));
        }

        var isSpeciesExists = await db.Species.AnyAsync(s => s.Id == request.SpeciesId);
        if (!isSpeciesExists)
        {
            return TypedResults
                .BadRequest($"Species with id: {request.SpeciesId} not exist");
        }

        var pet = new Pet()
        {
            Name = request.Name,
            Description = request.Description,
            Weight = request.Weight,
            SpeciesId = request.SpeciesId
        };
        await db.Pets.AddAsync(pet);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/api/pets/{pet.Id}", pet);
    }
}