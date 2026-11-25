using Web_453503.API.Abstractions;

namespace Web_453503.API;

public static class MapIEndpointsExtensions
{
    extension(WebApplication app)
    {
        public void MapIEndpoints()
        {
            var endpoints = typeof(Program).Assembly
                .GetTypes()
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract)
                .Select(type => Activator.CreateInstance(type) as IEndpoint)
                .ToArray();
            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }
        }
    }
}