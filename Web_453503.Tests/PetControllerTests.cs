using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_453503.Domain.Entities;
using Web_453503.Domain.Models;
using Web_453503.UI.Controllers;
using Web_453503.UI.Services.PetService;
using Web_453503.UI.Services.SpeciesService;

namespace Web_453503.Tests;

public class PetControllerTests
{
    private readonly ISpeciesService _speciesService;
    private readonly IPetService _petService;
    private readonly PetController _petController;

    public PetControllerTests()
    {
        _speciesService = Substitute.For<ISpeciesService>();
        _petService = Substitute.For<IPetService>();
        
        _petController = new PetController(_speciesService, _petService);

        _petController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };
    }
    
    [Fact]
    public async Task Index_SpeciesServiceFailed_ReturnsNotFound()
    {
        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>()
            {
                IsSuccessfully = false,
                ErrorMessage = "Species Not Found"
            });

        var result = await _petController.Index(null);
        
        var notFound =  Assert.IsType<NotFoundObjectResult>(result);
        
        Assert.Equal(404, notFound.StatusCode);
        Assert.Equal("Species Not Found", notFound.Value);
        
        await _petService.DidNotReceive()
            .GetPetListAsync(Arg.Any<string?>(), Arg.Any<int>());
    }
    
    [Fact]
    public async Task Index_PetServiceFailed_ReturnsNotFound()
    {
        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>
            {
                IsSuccessfully = true,
                Data = new List<Species>()
            });

        _petService.GetPetListAsync(Arg.Any<string?>(), Arg.Any<int>())
            .Returns(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = false,
                ErrorMessage = "Pet Not Found"
            });

        var result = await _petController.Index(null);
        
        var notFound =  Assert.IsType<NotFoundObjectResult>(result);
        
        Assert.Equal(404, notFound.StatusCode);
        Assert.Equal("Pet Not Found", notFound.Value);
    }
    
    [Fact]
    public async Task Index_Success_ReturnsSpeciesListInViewData()
    {
        var speciesList = new List<Species>
        {
            new Species
            {
                Name = "Cats",
                NormalizedName = "cats"
            },
            new Species
            {
                Name = "Dogs",
                NormalizedName = "dogs"
            }
        };

        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>
            {
                IsSuccessfully = true,
                Data = speciesList
            });

        _petService.GetPetListAsync(null, 1)
            .Returns(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = true,
                Data = new ListModel<Pet>()
            });
        
        var result = await _petController.Index(null);
        
        var view = Assert.IsType<ViewResult>(result);

        Assert.Same(speciesList, view.ViewData["speciesList"]);
    }
    
    [Theory]
    [InlineData(null, "All")]
    [InlineData("cats", "Cats")]
    [InlineData("dogs", "Dogs")]
    [InlineData("unknown", "All")]
    public async Task Index_DifferentSpecies_ReturnsCorrectSpeciesName(
        string? species,
        string expectedName)
    {
        var speciesList = new List<Species>
        {
            new Species
            {
                Name = "Cats",
                NormalizedName = "cats"
            },
            new Species
            {
                Name = "Dogs",
                NormalizedName = "dogs"
            }
        };

        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>
            {
                IsSuccessfully = true,
                Data = speciesList
            });

        _petService.GetPetListAsync(species, 1)
            .Returns(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = true,
                Data = new ListModel<Pet>()
            });

        var result = await _petController.Index(species);

        var view = Assert.IsType<ViewResult>(result);

        Assert.Equal(expectedName, view.ViewData["currentSpeciesName"]);
    }
    
    [Fact]
    public async Task Index_Success_ReturnsPetListInModel()
    {
        var petList = new List<Pet>
        {
            new Pet(),
            new Pet()
        };

        var petListModel = new ListModel<Pet>
        {
            Items = petList
        };

        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>
            {
                IsSuccessfully = true,
                Data = new List<Species>()
            });

        _petService.GetPetListAsync(null, 1)
            .Returns(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = true,
                Data = petListModel
            });

        var result = await _petController.Index(null);
        
        var view = Assert.IsType<ViewResult>(result);

        Assert.Same(petListModel, view.Model);
    }
    
    [Theory]
    [InlineData(null, 1)]
    [InlineData("cats", 2)]
    [InlineData("dogs", 3)]
    public async Task Index_CallsPetServiceWithCorrectArguments(
        string? species,
        int page)
    {
        _speciesService.GetSpeciesListAsync()
            .Returns(new ResponseData<List<Species>>
            {
                IsSuccessfully = true,
                Data = new List<Species>()
            });

        _petService.GetPetListAsync(species, page)
            .Returns(new ResponseData<ListModel<Pet>>
            {
                IsSuccessfully = true,
                Data = new ListModel<Pet>()
            });

        await _petController.Index(species, page);
        
        await _petService.Received(1)
            .GetPetListAsync(species, page);
    }
}