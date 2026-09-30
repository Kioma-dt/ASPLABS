namespace Web_453503_Avramenko.UI.Services.FileService;

public class LocalFileService(IWebHostEnvironment webHostEnvironment)
    : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var folder = Path.Combine(webHostEnvironment.WebRootPath, "images");

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        
        var extenstion = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName =
            Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
            + extenstion;
        
        var filePath = Path.Combine(folder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("images", fileName);
    }
}