using System.Diagnostics.Contracts;

namespace DruidsCornerApp.Models.RecipeExplorer;

/// <summary>
/// Compact Recipe that allows the recipe browser to list recipes with minimal data
/// </summary>
public class CompactRecipe
{
    /// <summary>
    /// Recipe's Alcoholic content
    /// </summary>
    public double Abv { get; set; } = 3.5;

    /// <summary>
    /// Recipe's bitterness grade
    /// </summary>
    public double Ibu { get; set; } = 35;

    /// <summary>
    /// Recipe's color
    /// </summary>
    public double Ebc { get; set; } = 30;

    public double Rating { get; set; } = 2.5;

    public bool Favorite { get; set; } = false;
    
    /// <summary>
    /// Brew date
    /// </summary>
    public string BrewDate { get; set; } = "2023";
    
    /// <summary>
    /// Recipe's name
    /// </summary>
    public string Name { get; set; } = "Recipe's name";
    /// <summary>
    /// Recipe's style, when known
    /// </summary>
    public string Style { get; set; } = "Unknown style";

    /// <summary>
    /// Recipe's original Brewer
    /// </summary>
    public string Brewer { get; set; } = "My name is Nobody";

    /// <summary>
    /// Recipe's image
    /// TODO : change that for something else, this is a temporary field to let the app load image from an url.
    /// </summary>
    public string ImageSource { get; set; } = string.Empty;

    /// <summary>
    /// Full Recipe object's index, used to retrieve the actual recipe from cache (or triggers a request to get it from remote)
    /// </summary>
    public uint RecipeId { get; set; } = UInt32.MaxValue;
}