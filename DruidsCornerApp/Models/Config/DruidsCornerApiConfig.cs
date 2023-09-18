using System.Text.Json.Serialization;

namespace DruidsCornerApp.Models.Config;

public record DruidsCornerApiConfig()
{
    public static string SectionName = "druidsCornerApi";
    
    [JsonPropertyName("domain")]
    public string Domain { get; set; }  = string.Empty;
}