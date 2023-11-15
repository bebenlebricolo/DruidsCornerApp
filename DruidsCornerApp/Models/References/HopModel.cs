namespace DruidsCornerApp.Models.References;

/// <summary>
/// Models a Hop with all its characteristics.
/// Most of this model is derived from what we could find on BeerMaverick website
/// </summary>
public record HopModel : RatableObject
{
    /// <summary>
    /// Hop name
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Hop data source link (usually, comes from BeerMaverick)
    /// </summary>
    public string Link { get; set; } = "";
    
    /// <summary>
    /// Hop purpose in beer brewing
    /// </summary>
    public HopAttribute Purpose { get; set; } = HopAttribute.Aromatic;
    
    /// <summary>
    /// Country of origin of this hop
    /// </summary>
    public string? Country { get; set; } = null;

    /// <summary>
    /// International code of this Hop, if any
    /// </summary>
    public string? InternationalCode { get; set; } = null;

    /// <summary>
    /// Cultivar Id, Brand
    /// </summary>
    public string? CultivarId { get; set; } = null;
    
    /// <summary>
    /// Ownership of Hop
    /// </summary>
    public string? Ownership { get; set; } = null;
    
    /// <summary>
    /// Origin text as per retrieved from data source
    /// </summary>
    public string OriginTxt { get; set; } = "";
    
    /// <summary>
    /// Flavor description text, as per retrieved from data source
    /// </summary>
    public string FlavorTxt { get; set; } = "";
    
    /// <summary>
    /// List of tags associated with this hop, things like "fruity", "citrus", "pineapple", "mango", etc.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Alpha acids usual range for this variety
    /// </summary>
    public NumericRange? AlphaAcids { get; set; } = null;

    /// <summary>
    /// Beta acids usual range for this variety
    /// </summary>
    public NumericRange? BetaAcids { get; set; } = null;

    /// <summary>
    /// Alpha acids / Beta acids usual ratio for this variety
    /// </summary>
    public TextRange? AlphaBetaRatio { get; set; } = null;

    /// <summary>
    /// Hop storage index, the higher, the better. Usually above 80/58%
    /// Expressed as a percentage * 100
    /// </summary>
    public double? HopStorageIndex { get; set; } = null;

    /// <summary>
    /// Normalized Co-Humulone concentration range for this variety
    /// </summary>
    public NumericRange? CoHumuloneNormalized { get; set; } = null;

    /// <summary>
    /// Total oils usual range for this variety
    /// </summary>
    public NumericRange? TotalOils { get; set; } = null;

    /// <summary>
    /// Usual Myrcene distribution for this variety
    /// </summary>
    public NumericRange? Myrcene { get; set; } = null;

    /// <summary>
    /// Usual Humulene distribution for this variety
    /// </summary>
    public NumericRange? Humulene { get; set; } = null;

    /// <summary>
    /// Usual Caryophyllene distribution for this variety
    /// </summary>
    public NumericRange? Caryophyllene { get; set; } = null;

    /// <summary>
    /// Usual Farnesene distribution for this variety
    /// </summary>
    public NumericRange? Farnesene { get; set; } = null;

    /// <summary>
    /// Usual OtherOils distribution for this variety
    /// </summary>
    public NumericRange? OtherOils { get; set; } = null;
    
    /// <summary>
    /// List of beer styles where this Hop is commonly used
    /// </summary>
    public List<string> BeerStyles { get; set; } = new();
    
    /// <summary>
    /// List of "Human picked" Hop substitutes for this variety.
    /// This is to oppose other ways of comparing hops such as the statistical approach used in the BeerMaverick Hop Comparison tool.
    /// </summary>
    public List<string> Substitutes { get; set; } = new();

    /// <summary>
    /// Flavor wheel (radar chart) for this Hop, as per retrieved from BeerMaverick website
    /// </summary>
    public RadarChart? RadarChart { get; set; } = null;
}

/// <summary>
/// Derived from BeerMaverick's Hop radar chart object
/// </summary>
public record RadarChart
{
    /// <summary>
    /// Citrus flavor, ranging from 0 to 5
    /// </summary>
    public int Citrus { get; set; } = 0;
    
    /// <summary>
    /// Tropical Fruit flavor, ranging from 0 to 5
    /// </summary>
    public int TropicalFruit { get; set; } = 0;
    
    /// <summary>
    /// Stone Fruit flavor, ranging from 0 to 5
    /// </summary>
    public int StoneFruit { get; set; } = 0;
    
    /// <summary>
    /// Berry flavor, ranging from 0 to 5
    /// </summary>
    public int Berry { get; set; } = 0;
    
    /// <summary>
    /// Floral flavor, ranging from 0 to 5
    /// </summary>
    public int Floral { get; set; } = 0;
    
    /// <summary>
    /// Grassy flavor, ranging from 0 to 5
    /// </summary>
    public int Grassy { get; set; } = 0;
    
    /// <summary>
    /// Herbal flavor, ranging from 0 to 5
    /// </summary>
    public int Herbal { get; set; } = 0;
    
    /// <summary>
    /// Spice flavor, ranging from 0 to 5
    /// </summary>
    public int Spice { get; set; } = 0;
    
    /// <summary>
    /// Resinous flavor, ranging from 0 to 5
    /// </summary>
    public int Resinous { get; set; } = 0;
}

/// <summary>
/// Hop attributes
/// </summary>
public enum HopAttribute
{   
    /// <summary>
    /// Hop is generally used for its Aromatic properties
    /// </summary>
    Aromatic,
    
    /// <summary>
    /// Hop is generally used for its Bittering properties
    /// </summary>
    Bittering, 
    
    /// <summary>
    /// Hop can be used either for its Bittering or Aromatic properties
    /// </summary>
    Hybrid
}