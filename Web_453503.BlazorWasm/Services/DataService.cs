using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using Web_453503.Domain.Entities;
using Web_453503.Domain.Models;

namespace Web_453503.BlazorWasm.Services;

public class DataService
    : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly IAccessTokenProvider _tokenProvider;

    public DataService(
        HttpClient httpClient,
        IConfiguration configuration,
        IAccessTokenProvider tokenProvider)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetValue<int>("ItemsPerPage", 3);
        _tokenProvider = tokenProvider;
    }
    
    public event Action? DataLoaded;
    public List<Species> Species { get; set; }
    public List<Pet> Pets { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public Species? SelectedSpecies { get; set; }
    
    public async Task GetPetsListAsync(int pageNo = 1)
    {
        var route = new StringBuilder("pets/");

        if (SelectedSpecies is not null)
        {
            route.Append(Uri.EscapeDataString(SelectedSpecies.NormalizedName));
        }
        
        List<KeyValuePair<string, string>> queryData = new();
        
        if (pageNo > 1)
        {
            queryData.Add(KeyValuePair.Create("page", pageNo.ToString()));
        };
        if (!_pageSize.Equals(3))
        {
            queryData.Add(KeyValuePair.Create("pageSize", _pageSize.ToString()));
        }
        if (queryData.Count > 0)
        {
            route.Append(QueryString.Create(queryData));
        }

        try
        {
            using var response = await SendAuthorizedRequestAsync(HttpMethod.Get, route.ToString());

            if (!response.IsSuccessStatusCode)
            {
                Success = false;
                ErrorMessage = await response.Content.ReadAsStringAsync();
                return;
            }

            var result = await response.Content
                .ReadFromJsonAsync<ResponseData<ListModel<Pet>>>();

            if (result is null)
            {
                Success = false;
                ErrorMessage = "Result is null";
                return;
            }

            Success = result.IsSuccessfully;
            ErrorMessage = result.ErrorMessage ?? "";

            if (Success && result.Data is not null)
            {
                Pets = result.Data.Items;
                CurrentPage = result.Data.CurrentPage;
                TotalPages = result.Data.TotalPages;
            }
            else
            {
                Success = false;
                ErrorMessage ??= "Failed to get pet list";
            }
        }
        catch (Exception ex)
        {
            Success = false;
            ErrorMessage = ex.Message;
        }
        finally
        {
            DataLoaded?.Invoke();
        }
    }

    public async Task GetSpeciesListAsync()
    {
        try
        {
            using var response = await SendAuthorizedRequestAsync(HttpMethod.Get, "species/");
            if (!response.IsSuccessStatusCode)
            {
                Success = false;
                ErrorMessage = await response.Content.ReadAsStringAsync();
                return;
            }

            var result = await response.Content
                .ReadFromJsonAsync<ResponseData<List<Species>>>();

            if (result is null)
            {
                Success = false;
                ErrorMessage = "Result is null";
                return;
            }

            Success = result.IsSuccessfully;
            ErrorMessage = result.ErrorMessage ?? "";

            if (Success && result.Data is not null)
            {
                Species = result.Data;
            }
            else
            {
                Success = false;
                ErrorMessage ??= "Failed to get species list";
            }
        }
        catch (Exception ex)
        {
            Success = false;
            ErrorMessage = ex.Message;
        }
        finally
        {
            DataLoaded?.Invoke();
        }
    }

    private async Task<HttpResponseMessage> SendAuthorizedRequestAsync(
        HttpMethod httpMethod,
        string requestUri)
    {
        var tokenRequest = await _tokenProvider.RequestAccessToken();

        if (!tokenRequest.TryGetToken(out var token))
        {
            throw new InvalidOperationException(
                "Access token is not available");
        }

        using var request = new HttpRequestMessage(httpMethod, requestUri);
        
        request.Headers
            .Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        
        return await _httpClient.SendAsync(request);
    }
}