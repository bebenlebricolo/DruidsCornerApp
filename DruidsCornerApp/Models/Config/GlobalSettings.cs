using System.Text.Json.Serialization;
namespace DruidsCornerApp.Models.Config;

public record GlobalSettings
{
    [JsonPropertyName("druidsCornerApi")] 
    public DruidsCornerApiConfig DruidsCornerApi { get; set; } = new DruidsCornerApiConfig();
}