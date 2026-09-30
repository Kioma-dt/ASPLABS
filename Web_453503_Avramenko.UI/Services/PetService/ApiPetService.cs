using System.Globalization;
using System.Text;
using System.Text.Json;
using Web_453503_Avramenko.UI.Services.Authentication;

namespace Web_453503_Avramenko.UI.Services.PetService;

public class ApiPetService
    : IPetService
{
    readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    readonly ILogger<ApiPetService> _logger;
    readonly ITokenAccessor _tokenAccessor;

    public ApiPetService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiPetService> logger,
        ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Get<int>();
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
        _tokenAccessor =  tokenAccessor;
    }
    
    public async Task<ResponseData<ListModel<Pet>>> GetPetListAsync(string? speciesNormalizedName, int pageNo = 1)
    {
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");
        
        if (speciesNormalizedName is not null)
        {
            urlString.Append($"{speciesNormalizedName}");
        };
        
        urlString.Append($"?page={pageNo}");

        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, false);
        }
        catch (Exception ex)
        {
            ResponseData<ListModel<Pet>>
                .Error($"Data not got from server. Error: {ex.Message}");
        }

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
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");
        urlString.Append($"{id}/");
        
        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, false);
        }
        catch (Exception ex)
        {
            ResponseData<ListModel<Pet>>
                .Error($"Data not got from server. Error: {ex.Message}");
        }
        
        var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
        
        if(response.IsSuccessStatusCode)
        {
            try
            {
                return ResponseData<Pet>.Success(await response
                    .Content
                    .ReadFromJsonAsync<Pet>
                        (_serializerOptions));
            }
            catch(JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return ResponseData<Pet>
                    .Error($"Error: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Data not got from server. Error:{response.StatusCode.ToString()}");
        return ResponseData<Pet>
            .Error($"Data not got from server. Error: {response.StatusCode.ToString()}");
    }

    public async Task UpdatePetAsync(
        Guid id, 
        Pet pet, 
        IFormFile? formFile)
    {
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");
        urlString.Append($"{id}/");
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(urlString.ToString())
        };

        var content = new MultipartFormDataContent();

        if (formFile is not null)
        {
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", formFile.FileName);
        }

        var data = new StringContent(JsonSerializer.Serialize(pet));
        content.Add(data, "pet");

        request.Content = content;
        
        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, false);
        }
        catch (Exception ex)
        {
            ResponseData<ListModel<Pet>>
                .Error($"Data not got from server. Error: {ex.Message}");
        }
        
        var response = await _httpClient.SendAsync(
            request,
            CancellationToken.None);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Object not Updated. Error: {response.StatusCode.ToString()}");
        }
    }

    public async Task DeletePetAsync(Guid id)
    {
        var urlString= new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");
        urlString.Append($"{id}/");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(urlString.ToString())
        };
        
        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, false);
        }
        catch (Exception ex)
        {
            ResponseData<ListModel<Pet>>
                .Error($"Data not got from server. Error: {ex.Message}");
        }
        
        var response = await _httpClient.SendAsync(
            request,
            CancellationToken.None);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Object not Deleted. Error: {response.StatusCode.ToString()}");
        }
    }

    public async Task<ResponseData<Pet>> CreatePetAsync(Pet pet, IFormFile? formFile)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = _httpClient.BaseAddress
        };
        
        var content = new MultipartFormDataContent();
        
        content.Add(new StringContent(pet.Name), "name");
        content.Add(new StringContent(pet.Description), "description");
        content.Add(new StringContent(pet.Weight.ToString(CultureInfo.InvariantCulture)), "weight");
        content.Add(new StringContent(pet.SpeciesId.ToString()), "speciesId");
        
        if (formFile is not null)
        {
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", formFile.FileName);
        }
        
        request.Content = content;
        
        try
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient, false);
        }
        catch (Exception ex)
        {
            ResponseData<ListModel<Pet>>
                .Error($"Data not got from server. Error: {ex.Message}");
        }
        
        var response = await _httpClient.SendAsync(
            request,
            CancellationToken.None);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response
                .Content
                .ReadFromJsonAsync<ResponseData<Pet>> (_serializerOptions);
            return responseData;
        }
        _logger.LogError($"-----> Object not created. Error: {response.StatusCode.ToString()}");
        return ResponseData<Pet>
            .Error($"Object not created. Error :{response.StatusCode.ToString()}");
    }

    // record CreatePetRequestDto(
    //     string Name,
    //     string Description,
    //     double Weight,
    //     Guid SpeciesId,
    //     IFormFile? File);
}