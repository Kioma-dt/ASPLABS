namespace Web_453503_Avramenko.UI.Services.FileService;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}