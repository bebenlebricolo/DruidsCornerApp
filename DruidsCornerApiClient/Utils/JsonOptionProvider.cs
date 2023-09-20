using System.Text.Json;
using System.Text.Json.Serialization;

namespace DruidsCornerApiClient.Utils;

public static class JsonOptionProvider
{
    public static JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }
}