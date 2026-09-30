using NuGet.Protocol.Plugins;

namespace Web_453503_Avramenko.API.UseCases;

public sealed record SaveImage(IFormFile File)
    : IRequest<string>;

public class SaveImageHandler(
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<SaveImage, string>
{
    public async Task<string> Handle(
        SaveImage request, 
        CancellationToken cancellationToken)
    {
        var folder = Path.Combine(webHostEnvironment.WebRootPath, "images");

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        
        var extenstion = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        var fileName =
            Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
            + extenstion;
        
        var filePath = Path.Combine(folder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        var scheme = httpContextAccessor.HttpContext.Request.Scheme;
        var host = httpContextAccessor.HttpContext.Request.Host;
        var baseUrl = new Uri($"{scheme}://{host}");
        var imageFolderPathUrl = new Uri(baseUrl, "images");
        var imageFullUrl = new Uri(imageFolderPathUrl, fileName);
        return imageFullUrl.ToString();
    }
}