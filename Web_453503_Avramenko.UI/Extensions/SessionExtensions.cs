using System.Text.Json;

namespace Web_453503_Avramenko.UI.Extensions;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        string json = JsonSerializer.Serialize(value);
        session.SetString(key, json);
    }

    public static T? Get<T>(this ISession session, string key)
    {
        string? json = session.GetString(key);

        if (json is null)
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(json);
    }
}