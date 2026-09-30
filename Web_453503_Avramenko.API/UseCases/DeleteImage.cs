namespace Web_453503_Avramenko.API.UseCases;

public sealed record DeleteImage(string Url)
    : IRequest;
    
    public class DeleteImageHandler(IWebHostEnvironment  webHostEnvironment)
        : IRequestHandler<DeleteImage>
    {
        public async Task Handle(
            DeleteImage request, 
            CancellationToken cancellationToken)
        {
            var fileName = request.Url.Split('/').Last();
            
            var folder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            
            var filePath = Path.Combine(folder, fileName);
            
            File.Delete(filePath);
        }
    }