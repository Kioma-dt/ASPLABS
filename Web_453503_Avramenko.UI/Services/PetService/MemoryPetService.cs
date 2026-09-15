namespace Web_453503_Avramenko.UI.Services.PetService;

public class MemoryPetService
    : IPetService
{
    readonly List<Pet> _pets = new();
    readonly List<Species> _species = new();
    

    public MemoryPetService(ISpeciesService speciesService)
    {
        _species = speciesService.GetSpeciesListAsync()
                       .Result
                       .Data
                   ?? _species;
        
        this.SetupData();
    }
    
    public Task<ResponseData<ListModel<Pet>>> GetPetListAsync(
        string? speciesNormalizedName, 
        int pageNo = 1)
    {
        var items = new ListModel<Pet>()
        {
            Items = _pets,
            CurrentPage = pageNo,
            TotalPages = pageNo
        };

        var result = ResponseData<ListModel<Pet>>.Success(items);

        return Task.FromResult(result);
    }

    public Task<ResponseData<Pet>> GetPetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePetAsync(Guid id, Pet pet, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeletePetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Pet>> CreatePetAsync(Pet pet, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
    
    void SetupData()
    {
        _pets.AddRange(new List<Pet>()
        {
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Tom",
                Description = "Very good cat",
                Weight = 5,
                Species = _species.Find(s => s.NormalizedName.Equals("cat")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = "../images/Tom.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Name = "Luna",
                Description = "Calm and affectionate cat",
                Weight = 4.2,
                Species = _species.Find(s => s.NormalizedName.Equals("cat")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = "../images/Luna.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                Name = "Oliver",
                Description = "Playful and curious cat",
                Weight = 6.1,
                Species = _species.Find(s => s.NormalizedName.Equals("cat")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = "../images/Oliver.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Name = "Milo",
                Description = "Friendly and energetic cat",
                Weight = 5.5,
                Species = _species.Find(s => s.NormalizedName.Equals("cat")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("cat"))?.Id,
                Image = "../images/Milo.jpg"
            },

            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000005"),
                Name = "Buddy",
                Description = "Friendly and playful dog",
                Weight = 12,
                Species = _species.Find(s => s.NormalizedName.Equals("dog")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = "../images/Buddy.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Name = "Max",
                Description = "Loyal and energetic dog",
                Weight = 18.5,
                Species = _species.Find(s => s.NormalizedName.Equals("dog")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = "../images/Max.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000007"),
                Name = "Bella",
                Description = "Gentle and intelligent dog",
                Weight = 9.8,
                Species = _species.Find(s => s.NormalizedName.Equals("dog")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = "../images/Bella.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000008"),
                Name = "Rocky",
                Description = "Active and brave dog",
                Weight = 22,
                Species = _species.Find(s => s.NormalizedName.Equals("dog")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("dog"))?.Id,
                Image = "../images/Rocky.jpg"
            },

            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000009"),
                Name = "Charlie",
                Description = "Calm and curious pigeon",
                Weight = 0.4,
                Species = _species.Find(s => s.NormalizedName.Equals("pigeon")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = "../images/Charlie.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000010"),
                Name = "Sky",
                Description = "Active and social pigeon",
                Weight = 0.35,
                Species = _species.Find(s => s.NormalizedName.Equals("pigeon")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = "../images/Sky.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000011"),
                Name = "Pearl",
                Description = "Quiet and gentle pigeon",
                Weight = 0.42,
                Species = _species.Find(s => s.NormalizedName.Equals("pigeon")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("pigeon"))?.Id,
                Image = "../images/Pearl.jpg"
            },

            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000012"),
                Name = "Nibbles",
                Description = "Small and energetic hamster",
                Weight = 0.15,
                Species = _species.Find(s => s.NormalizedName.Equals("hamster")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = "../images/Nibbles.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000013"),
                Name = "Peanut",
                Description = "Cute and curious hamster",
                Weight = 0.13,
                Species = _species.Find(s => s.NormalizedName.Equals("hamster")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = "../images/Peanut.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000014"),
                Name = "Cookie",
                Description = "Friendly and active hamster",
                Weight = 0.17,
                Species = _species.Find(s => s.NormalizedName.Equals("hamster")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("hamster"))?.Id,
                Image = "../images/Cookie.jpg"
            },

            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000015"),
                Name = "Coco",
                Description = "Gentle and friendly guinea pig",
                Weight = 0.8,
                Species = _species.Find(s => s.NormalizedName.Equals("guinea-pig")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = "../images/Coco.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000016"),
                Name = "Mochi",
                Description = "Calm and adorable guinea pig",
                Weight = 0.95,
                Species = _species.Find(s => s.NormalizedName.Equals("guinea-pig")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = "../images/Mochi.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000017"),
                Name = "Pip",
                Description = "Small and playful guinea pig",
                Weight = 0.75,
                Species = _species.Find(s => s.NormalizedName.Equals("guinea-pig")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("guinea-pig"))?.Id,
                Image = "../images/Pip.jpg"
            },

            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000018"),
                Name = "Webster",
                Description = "Quiet and fascinating pet spider",
                Weight = 0.02,
                Species = _species.Find(s => s.NormalizedName.Equals("spider")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = "../images/Webster.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000019"),
                Name = "Shadow",
                Description = "Small and interesting spider",
                Weight = 0.015,
                Species = _species.Find(s => s.NormalizedName.Equals("spider")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = "../images/Shadow.jpg"
            },
            new Pet()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000020"),
                Name = "Ruby",
                Description = "Colorful and calm pet spider",
                Weight = 0.018,
                Species = _species.Find(s => s.NormalizedName.Equals("spider")),
                SpeciesId = _species.Find(s => s.NormalizedName.Equals("spider"))?.Id,
                Image = "../images/Ruby.jpg"
            }
        });
    }
}