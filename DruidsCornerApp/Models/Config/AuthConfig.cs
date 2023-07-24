using System.Text.Json.Serialization;
namespace DruidsCornerApp.Models.Config;

/// <summary>
/// Represents Firebase's authentication configuration.
/// </summary>
public record AuthConfig
{
    /// <summary>
    /// Firebase Project's Api Key
    /// </summary>
    [JsonPropertyName("FIREBASE_API_KEY")]
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Firebase Project's Auth domain, where domain is used like this : {domain}.firebaseapp.com
    /// </summary>
    [JsonPropertyName("FIREBASE_AUTH_DOMAIN")]
    public string AuthDomain { get; set; }  = string.Empty;
    
    /// <summary>
    /// JWT Token required scopes.
    /// This is required in order to be able to retrieve a token that has
    /// the right scopes (and more generally to retrieve a jwt token (aka "ID Token") from google that
    /// allows us to access CloudRun apis ; we can't call the CloudRun apis just with the Google's AccessToken
    /// </summary>
    [JsonPropertyName("JWT_SCOPES")]
    public List<string> JwtScopes { get; set; } = new List<string>();
}