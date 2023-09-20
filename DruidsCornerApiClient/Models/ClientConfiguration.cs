using System.Text.Json.Serialization;

namespace DruidsCornerApiClient.Models;

public record ClientConfiguration()
{
    [JsonPropertyName("domain")] 
    public string Domain { get; set; } = string.Empty;
}