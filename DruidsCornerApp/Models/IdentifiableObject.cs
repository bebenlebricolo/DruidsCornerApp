namespace DruidsCornerApp.Models;

/// <summary>
/// Identifiable object record gives common properties that all objects consumed from a database
/// will need to inherit.
/// </summary>
public record IdentifiableObject
{
    /// <summary>
    /// Object global unique identifier
    /// </summary>
    public string Id { get; set; } = "";
}
