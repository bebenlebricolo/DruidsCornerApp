namespace DruidsCornerApp.Models.Authentication;

/// <summary>
/// Encodes a very basic response from the Auth Gateway server
/// </summary>
public record AuthGatewayResponse()
{
    /// <summary>
    /// Wraps the retrieved IdToken
    /// </summary>
    public string IdToken { get; set; } = "";
}