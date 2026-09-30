namespace Web_453503_Avramenko.UI.Services.Authentication;

public interface ITokenAccessor
{
    Task SetAuthorizationHeaderAsync(HttpClient httpClient,
        bool isClient);
}