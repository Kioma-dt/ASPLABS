namespace Web_453503.UI.Services.FileService;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}