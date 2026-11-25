using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Web_453503.API.Data;
using Web_453503.API.Features;
using Web_453503.Domain.Entities;
using Web_453503.Domain.Models;

namespace Web_453503.Tests;

public class GetAllPetsTests
    : IDisposable, IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;
    private readonly ServiceProvider _serviceProvider;
    private readonly HybridCache _cache;

    public GetAllPetsTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        var service = new ServiceCollection();

        service.AddLogging();
        service.AddHybridCache();
        
        _serviceProvider = service.BuildServiceProvider();
        
        _cache = _serviceProvider.GetRequiredService<HybridCache>();

        using var db = CreateContext();
        
        db.Database.EnsureCreated();

        var catsId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var dogsId = Guid.Parse("10000000-0000-0000-0000-000000000002");

        db.Species.AddRange(
            new Species
            {
                Id = catsId,
                Name = "Cats",
                NormalizedName = "cats"
            },
            new Species
            {
                Id = dogsId,
                Name = "Dogs",
                NormalizedName = "dogs"
            }
        );

        db.Pets.AddRange(
            new Pet
            {
                Id = CreatePetId(1),
                Name = "Cat 1",
                Description = "British Shorthair cat",
                Weight = 4.5,
                SpeciesId = catsId
            },
            new Pet
            {
                Id = CreatePetId(2),
                Name = "Cat 2",
                Description = "Siamese cat",
                Weight = 3.8,
                SpeciesId = catsId
            },
            new Pet
            {
                Id = CreatePetId(3),
                Name = "Cat 3",
                Description = "Maine Coon cat",
                Weight = 7.2,
                SpeciesId = catsId
            },
            new Pet
            {
                Id = CreatePetId(4),
                Name = "Cat 4",
                Description = "Persian cat",
                Weight = 4.1,
                SpeciesId = catsId
            },
            new Pet
            {
                Id = CreatePetId(5),
                Name = "Dog 1",
                Description = "German Shepherd dog",
                Weight = 32.5,
                SpeciesId = dogsId
            },
            new Pet
            {
                Id = CreatePetId(6),
                Name = "Dog 2",
                Description = "Golden Retriever dog",
                Weight = 28.3,
                SpeciesId = dogsId
            },
            new Pet
            {
                Id = CreatePetId(7),
                Name = "Dog 3",
                Description = "Beagle dog",
                Weight = 10.5,
                SpeciesId = dogsId
            }
        );

        db.SaveChanges();

    }

    private AppDbContext CreateContext()
    {
        return new AppDbContext(_options);
    }

    private static ResponseData<ListModel<Pet>> GetResponse(IResult result)
    {
        var ok = Assert.IsType<
            Ok<ResponseData<ListModel<Pet>>>>(result);
        
        return Assert.IsType<ResponseData<ListModel<Pet>>>(ok.Value);
    }
    
    private static Guid CreatePetId(int id)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{id:D12}");
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        await _connection.DisposeAsync();
    }
    
    
    [Fact]
    public async Task Handler_Default_ReturnsFirstPageOfThreeItems()
    {
        await using var db = CreateContext();
        
        var result = await GetAllPets.Handler(db, _cache, null);
        
        var response = GetResponse(result);

        Assert.True(response.IsSuccessfully);
        Assert.NotNull(response.Data);

        Assert.Equal(1, response.Data.CurrentPage);
        Assert.Equal(3, response.Data.Items.Count);
        Assert.Equal(3, response.Data.TotalPages);

        Assert.Equal(
            new[]
            {
                CreatePetId(1),
                CreatePetId(2), 
                CreatePetId(3),
            },
            response.Data.Items.Select(p => p.Id));
    }
    
    [Theory]
    [InlineData(1, new int[] { 1, 2, 3 })]
    [InlineData(2, new int[] { 4, 5, 6 })]
    [InlineData(3, new int[] { 7 })]
    public async Task Handler_ReturnsRequestedPage(
        int page,
        int[] expectedIds)
    {
        await using var db = CreateContext();

        var result = await GetAllPets.Handler(
            db, _cache, null, page);

        var response = GetResponse(result);

        Assert.True(response.IsSuccessfully);
        Assert.NotNull(response.Data);

        Assert.Equal(page, response.Data.CurrentPage);
        Assert.Equal(3, response.Data.TotalPages);
        Assert.Equal(expectedIds.Length, response.Data.Items.Count);

        Assert.Equal(
            expectedIds.Select(CreatePetId).ToArray(),
            response.Data.Items.Select(p => p.Id));
    }
    
    [Theory]
    [InlineData("cats", 1, 2, new int[] { 1, 2, 3 })]
    [InlineData("cats", 2, 2, new int[] { 4 })]
    [InlineData("dogs", 1, 1, new int[] { 5, 6, 7 })]
    public async Task Handler_FiltersBySpecies(
        string species,
        int page,
        int expectedTotalPages,
        int[] expectedIds)
    {
        await using var db = CreateContext();

        var result = await GetAllPets.Handler(
            db, _cache, species, page);

        var response = GetResponse(result);

        Assert.True(response.IsSuccessfully);
        Assert.NotNull(response.Data);

        Assert.Equal(page, response.Data.CurrentPage);
        Assert.Equal(expectedTotalPages, response.Data.TotalPages);
        Assert.Equal(expectedIds.Length, response.Data.Items.Count);

        Assert.Equal(
            expectedIds.Select(CreatePetId).ToArray(),
            response.Data.Items.Select(p => p.Id));

        Assert.All(response.Data.Items, pet =>
        {
            Assert.Equal(species, pet.Species!.NormalizedName);
        });
    }
    
    [Theory]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task Handler_LimitsMaximumPageSize(int pageSize)
    {
        await using var db = CreateContext();

        var catsId = db.Species
            .First(s => s.NormalizedName == "cats").Id;
        
        for (int i = 8; i <= 25; i++)
        {
            db.Pets.Add(new Pet
            {
                Id = CreatePetId(i),
                Name = $"Cat {i}",
                Description = $"Test pet {i}",
                Weight = 4.5,
                SpeciesId = catsId
            });
        }

        await db.SaveChangesAsync();

        var result = await GetAllPets.Handler(
            db, _cache, null, 1, pageSize);
        
        var response = GetResponse(result);

        Assert.True(response.IsSuccessfully);
        Assert.NotNull(response.Data);

        Assert.Equal(1, response.Data.CurrentPage);
        Assert.Equal(20, response.Data.Items.Count);
        Assert.Equal(2, response.Data.TotalPages);

        Assert.Equal(
            Enumerable.Range(1, 20).Select(CreatePetId),
            response.Data.Items.Select(p => p.Id));
    }
    
    [Theory]
    [InlineData(null, 4)]
    [InlineData(null, 100)]
    [InlineData("cats", 3)]
    [InlineData("dogs", 2)]
    public async Task Handler_PageOutOfRange_ReturnsFailure(
        string? species,
        int page)
    {
        await using var db = CreateContext();
        
        var result = await GetAllPets.Handler(
            db, _cache, species, page);
        
        var response = GetResponse(result);

        Assert.False(response.IsSuccessfully);
    }
    
    [Fact]
    public async Task Handler_RepeatedRequest_ReturnsCachedData()
    {
        await using var db = CreateContext();

        var firstResult = await GetAllPets.Handler(
            db, _cache, null);

        var firstResponse = GetResponse(firstResult);

        Assert.True(firstResponse.IsSuccessfully);
        
        await db.Pets.ExecuteDeleteAsync();

        Assert.Equal(0, await db.Pets.CountAsync());

        var secondResult = await GetAllPets.Handler(
            db, _cache, null);
        
        var secondResponse = GetResponse(secondResult);

        Assert.True(secondResponse.IsSuccessfully);
        Assert.NotNull(secondResponse.Data);

        Assert.Equal(3, secondResponse.Data.Items.Count);
        Assert.Equal(3, secondResponse.Data.TotalPages);
    }
}