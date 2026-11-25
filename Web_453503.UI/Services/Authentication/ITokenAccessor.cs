namespace Web_453503.UI.Services.Authentication;

public interface ITokenAccessor
{
    Task SetAuthorizationHeaderAsync(HttpClient httpClient,
        bool isClient);
}