namespace Web_453503_Avramenko.UI.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["x-requested-with"]
            .ToString()
            .ToLower()
            .Equals("xmlhttprequest");
    }
}