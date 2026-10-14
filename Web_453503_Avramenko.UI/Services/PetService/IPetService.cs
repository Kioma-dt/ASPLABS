namespace Web_453503_Avramenko.UI.Services.PetService;

public interface IPetService
{ 
    public Task<ResponseData<ListModel<Pet>>> GetPetListAsync(
        string? speciesNormalizedName, 
        int pageNo=1);
    
    public Task<ResponseData<Pet>> GetPetByIdAsync(Guid id);

    public Task UpdatePetAsync(
        Guid id, 
        Pet pet,
        IFormFile? formFile);
    
    public Task DeletePetAsync(Guid id);
    
    public Task<ResponseData<Pet>> CreatePetAsync(
        Pet pet, 
        IFormFile? formFile);
}