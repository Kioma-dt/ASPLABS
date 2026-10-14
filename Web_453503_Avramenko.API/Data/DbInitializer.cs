using Microsoft.EntityFrameworkCore;

namespace Web_453503_Avramenko.API.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await context.Database.MigrateAsync();

        var imagePrefix = app.Configuration.GetSection("ImagePrefix").Value;
        
        context.Species.AddRange( new List<Species>()
        {
            new Species()
            {
                Name = "Cat",
                NormalizedName = "cat"
            },
            new Species()
            {
                Name = "Dog",
                NormalizedName = "dog"
            },
            new Species()
            {
                Name = "Pigeon",
                NormalizedName = "pigeon"
            },
            new Species()
            {
                Name = "Hamster",
                NormalizedName = "hamster"
            },
            new Species()
            {
                Name = "Guinea Pig",
                NormalizedName = "guinea-pig"
            },
            new Species()
            {
                Name = "Spider",
                NormalizedName = "spider"
            },
        });

        await context.SaveChangesAsync();
        
        context.Pets.AddRange(new List<Pet>()
        {
            new Pet()
            {
                Name = "Tom",
                Description = "Very good cat",
                Weight = 5,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = imagePrefix + "images/Tom.jpeg"
            },
            new Pet()
            {
                Name = "Luna",
                Description = "Calm and affectionate cat",
                Weight = 4.2,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = imagePrefix +"images/Luna.jpeg"
            },
            new Pet()
            {
                Name = "Oliver",
                Description = "Playful and curious cat",
                Weight = 6.1,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = imagePrefix + "images/Oliver.jpeg"
            },
            new Pet()
            {
                Name = "Milo",
                Description = "Friendly and energetic cat",
                Weight = 5.5,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = imagePrefix + "images/Milo.jpeg"
            },

            new Pet()
            {
                Name = "Buddy",
                Description = "Friendly and playful dog",
                Weight = 12,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = imagePrefix + "images/Buddy.jpeg"
            },
            new Pet()
            {
                Name = "Max",
                Description = "Loyal and energetic dog",
                Weight = 18.5,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = imagePrefix + "images/Max.jpeg"
            },
            new Pet()
            {
                Name = "Bella",
                Description = "Gentle and intelligent dog",
                Weight = 9.8,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = imagePrefix + "images/Bella.jpeg"
            },
            new Pet()
            {
                Name = "Rocky",
                Description = "Active and brave dog",
                Weight = 22,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = imagePrefix + "images/Rocky.jpeg"
            },

            new Pet()
            {
                Name = "Charlie",
                Description = "Calm and curious pigeon",
                Weight = 0.4,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = imagePrefix + "images/Charlie.jpeg"
            },
            new Pet()
            {
                Name = "Sky",
                Description = "Active and social pigeon",
                Weight = 0.35,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = imagePrefix + "images/Sky.jpeg"
            },
            new Pet()
            {
                Name = "Pearl",
                Description = "Quiet and gentle pigeon",
                Weight = 0.42,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = imagePrefix + "images/Pearl.jpeg"
            },

            new Pet()
            {
                Name = "Nibbles",
                Description = "Small and energetic hamster",
                Weight = 0.15,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = imagePrefix + "images/Nibbles.jpeg"
            },
            new Pet()
            {
                Name = "Peanut",
                Description = "Cute and curious hamster",
                Weight = 0.13,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = imagePrefix + "images/Peanut.jpeg"
            },
            new Pet()
            {
                Name = "Cookie",
                Description = "Friendly and active hamster",
                Weight = 0.17,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = imagePrefix + "images/Cookie.jpeg"
            },

            new Pet()
            {
                Name = "Coco",
                Description = "Gentle and friendly guinea pig",
                Weight = 0.8,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = imagePrefix + "images/Coco.jpeg"
            },
            new Pet()
            {
                Name = "Mochi",
                Description = "Calm and adorable guinea pig",
                Weight = 0.95,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = imagePrefix + "images/Mochi.jpeg"
            },
            new Pet()
            {
                Name = "Pip",
                Description = "Small and playful guinea pig",
                Weight = 0.75,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = imagePrefix + "images/Pip.jpeg"
            },

            new Pet()
            {
                Name = "Webster",
                Description = "Quiet and fascinating pet spider",
                Weight = 0.02,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = imagePrefix + "images/Webster.jpeg"
            },
            new Pet()
            {
                Name = "Shadow",
                Description = "Small and interesting spider",
                Weight = 0.015,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = imagePrefix + "images/Shadow.jpeg"
            },
            new Pet()
            {
                Name = "Ruby",
                Description = "Colorful and calm pet spider",
                Weight = 0.018,
                SpeciesId = context.Species.FirstOrDefault(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = imagePrefix + "images/Ruby.jpeg"
            }
        });

        await context.SaveChangesAsync();
    }
}