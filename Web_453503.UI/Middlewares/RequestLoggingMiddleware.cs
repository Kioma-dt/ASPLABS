namespace Web_453503.UI.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next, 
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var url = context.Request.PathBase
                  + context.Request.Path;
        
        context.Response.OnCompleted(() =>
        {
            var statusCode = context.Response.StatusCode;

            if (statusCode < 200 || statusCode >= 300)
            {
            
                _logger.LogInformation(
                    "---> request {RequestUrl} returns {StatusCode}",
                    url.ToString(), 
                    statusCode);
            }
            
            return Task.CompletedTask;
        });
        
        await _next(context);
        
    }
}