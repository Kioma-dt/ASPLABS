using System.Text;
using System.Text.Json;

namespace Web_453503_Avramenko.UI.Services.PetService;

public class ApiPetService
    : IPetService
{
    readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    readonly ILogger<ApiPetService> _logger;

    public ApiPetService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiPetService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Get<int>();
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    
    public async Task<ResponseData<ListModel<Pet>>> GetPetListAsync(string? speciesNormalizedName, int pageNo = 1)
    {
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}/");
        
        if (speciesNormalizedName is not null)
        {
            urlString.Append($"{speciesNormalizedName}");
        };
        
        urlString.Append($"?page={pageNo}");

        var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
        
        if(response.IsSuccessStatusCode)
        {
            try
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<ListModel<Pet>>>
                        (_serializerOptions);
            }
            catch(JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return ResponseData<ListModel<Pet>>
                    .Error($"Error: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Data not got from server. Error:{response.StatusCode.ToString()}");
        return ResponseData<ListModel<Pet>>
            .Error($"Data not got from server. Error: {response.StatusCode.ToString()}");
    }

    public async Task<ResponseData<Pet>> GetPetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdatePetAsync(Guid id, Pet pet, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<Pet>> CreatePetAsync(Pet pet, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}