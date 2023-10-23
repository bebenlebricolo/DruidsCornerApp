namespace DruidsCornerApp.Models;

/// <summary>
/// Identifiable object record gives common properties that all objects consumed from a database
/// will need to inherit.
/// </summary>
public record RatableObject : IdentifiableObject
{
    /// <summary>
    /// Global rating, calculated using the list of UserRatings.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Available individual ratings for each user that left a note.
    /// </summary>
    public List<UserRating> UserRatings { get; set; } = new();
}
