using System.Text;
using System.Text.Json;
using Web_453503.UI.Services.PetService;

namespace Web_453503.UI.Services.SpeciesService;

public class ApiSpeciesService
    : ISpeciesService
{
    readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    readonly ILogger<ApiSpeciesService> _logger;
    
    public ApiSpeciesService(
        HttpClient httpClient,
        ILogger<ApiSpeciesService> logger)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    
    public async Task<ResponseData<List<Species>>> GetSpeciesListAsync()
    {
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");
        
        var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
        
        if(response.IsSuccessStatusCode)
        {
            try
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<List<Species>>>
                        (_serializerOptions);
            }
            catch(JsonException ex)
            {
                _logger.LogError(ex, 
                    "Failed to describe list");
                return ResponseData<List<Species>>
                    .Error($"Ошибка: {ex.Message}");
            }
        }
        
        _logger.LogError(
            "Data not got from server. Error: {ServerError}",
            response.StatusCode.ToString());
        return ResponseData<List<Species>>
            .Error($"Data not got from service. Error: {response.StatusCode.ToString()}");
    }
}