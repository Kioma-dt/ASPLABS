using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Web_453503_Avramenko.UI.HelperClasses;

namespace Web_453503_Avramenko.UI.Services.Authentication;

public class KeycloakTokenAccessor(
    IHttpContextAccessor httpContextAccessor,
    IOptions<KeycloakData> options,
    HttpClient httpClient)
    : ITokenAccessor
{
    public async Task SetAuthorizationHeaderAsync(
        HttpClient httpClient, 
        bool isClient)
    {
        string token = isClient
            ? await GetClientToken()
            : await GetUserToken();
        
        httpClient.DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("bearer", token);
    }

    async Task<string> GetUserToken()
    {
        var context = httpContextAccessor.HttpContext;
        var authSession = await context.AuthenticateAsync("keycloak");

        if (authSession?.Principal is null)
        {
            throw new AuthenticationFailedException("User is unauthorized");
        }
        
        return await context.GetTokenAsync("keycloak", "access_token");
    }

    async Task<string> GetClientToken()
    {
        var requestUri =
            $"{options.Value.Host}/realms/{options.Value.Realm}/protocol/openid-connect/token";

        HttpContent content = new FormUrlEncodedContent([
                new KeyValuePair<string, string>
                    ("client_id", options.Value.ClientId),
                new KeyValuePair<string, string>
                    ("grant_type", "client_credentials"),
                new KeyValuePair<string, string>
                    ("client_secret", options.Value.ClientSecret)]);
        
        var response = await httpClient.PostAsync(requestUri, content);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }

        var jsonObject = await response.Content.ReadAsStringAsync();
        return JsonObject.Parse(jsonObject)["access_token"].GetValue<string>();
    }
}