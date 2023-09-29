namespace DruidsCornerApp.Models.MainContext;

/// <summary>
/// Compact Recipe that allows the recipe browser to list recipes with minimal data
/// </summary>
public class CompactHopModel
{
    /// <summary>
    /// Recipe's Alcoholic content
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Hop alpha acid content range
    /// </summary>
    public string AlphaAcids { get; set; } = "";
    
    /// <summary>
    /// Recipe's bitterness grade
    /// </summary>
    public string Purpose { get; set; } = "Aromatic";

    /// <summary>
    /// States the amount of this hop that we currently have in stock
    /// </summary>
    public double StockedAmount { get; set; } = 153;

    /// <summary>
    /// States the global rating of this resource amongst the community
    /// </summary>
    public double Rating { get; set; } = 2.5;

    /// <summary>
    /// States whether this resource was set as a favorite by the user or not
    /// </summary>
    public bool Favorite { get; set; } = false;
}