namespace DruidsCornerApp.Models;

/// <summary>
/// Encodes a rating left by a user
/// </summary>
public record UserRating
{
    /// <summary>
    /// Rating left by the user (ranging from 0 to 5)
    /// </summary>
    public uint Rating { get; set; } = 0;

    /// <summary>
    /// User Unique ID in the database system / IdentityProvider
    /// </summary>
    public string UserId { get; set; } = "";

    /// <summary>
    /// User left an optional comment on the rated object
    /// </summary>
    public string? Comment { get; set; } = null;
}