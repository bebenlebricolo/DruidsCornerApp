namespace DruidsCornerApp.Models.References;

/// <summary>
/// Models a Yeast with all its characteristics.
/// Most of this model is derived from what we could find on BeerMaverick website
/// </summary>
public record YeastModel : IdentifiableObject
{
    /// <summary>
    /// Yeast name
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Yeast data source link (usually, comes from BeerMaverick)
    /// </summary>
    public string Link { get; set; } = "";
    
    /// <summary>
    /// Yeast data source link (usually, comes from BeerMaverick)
    /// </summary>
    public string Brand { get; set; } = "";
    
    /// <summary>
    /// Yeast packaging method.
    /// Can sometimes be provided either as one or the other method
    /// but this information is not present in the original dataset.
    /// </summary>
    public Packaging Packaging { get; set; } = Packaging.Unknown;
    
    /// <summary>
    /// Textual description of what this yeast is.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// List of tags associated with this yeast, things like "clove", "pepper", "honey", etc.
    /// </summary>
    public List<string> Tags { get; set; } = new();
    
    /// <summary>
    /// Yeast tolerance towards alcoholic content.
    /// Past a certain point, ethanol is toxic to yeast cells and they either slow down or die.
    /// </summary>
    public double? AlcoholTolerance { get; set; } = null;

    /// <summary>
    /// Attenuation is the capacity of a yeast population to
    /// consume hydrocarbons from a solution. Expressed as a range of percentage (usually around 70 - 80%).
    /// </summary>
    public NumericRange? Attenuation { get; set; } = null;
    
    /// <summary>
    /// Flocculation properties of this yeast strain.
    /// </summary>
    public Flocculation Flocculation { get; set; } = Flocculation.Medium;
    
    /// <summary>
    /// Optimal working temperature range for this yeast strain.
    /// This temperature range is the optimal one regarding beer production, usually yeast can withstand (and are actually happier!)
    /// higher temperatures (best for breeding) but this gives off-flavors in beer. They also can work under lower temperatures,
    /// but often they won't work as fast / won't attenuate a beer completely, and just go to sleep otherwise. Whoops !
    /// </summary>
    public NumericRange? OptimalTemperature { get; set; } = null;
    
    /// <summary>
    /// Flavor description text, as per retrieved from data source
    /// </summary>
    public string FlavorTxt { get; set; } = "";

    /// <summary>
    /// List of yeast that are often compared to this one.
    /// Either on the same style, or from another brand with more or less the same signature.
    /// This can be used to find potential substitutes.
    /// </summary>
    public List<string> ComparableYeasts { get; set; } = new();
}

/// <summary>
/// Derived from BeerMaverick's Yeast radar chart object
/// </summary>
public enum Flocculation
{
    /// <summary>
    /// Yeast tends not to clump up together. Stays in suspension for very extended periods of time.
    /// </summary>
    VeryLow,
    
    /// <summary>
    /// Yeast tends to clump up, but very slowly.
    /// </summary>
    Low,
    
    /// <summary>
    /// Yeast has quite poor clumping up capabilities 
    /// </summary>
    LowMedium,
    
    /// <summary>
    /// Yeast has average clumping up capabilities
    /// </summary>
    Medium,
    
    /// <summary>
    /// Yeast tends to clump up quite easily
    /// </summary>
    MediumHigh,
    
    /// <summary>
    /// Yeast clumps up quite rapidly
    /// </summary>
    High,
    
    /// <summary>
    /// Yeast clumps up super fast, in few days they are already sleeping !
    /// </summary>
    VeryHigh
}

/// <summary>
/// Yeast packaging
/// </summary>
public enum Packaging
{   
    /// <summary>
    /// Yeast is provided in a liquid form, inside a watertight package.
    /// It needs to be kept in a dark and dry environment, low temperatures (like in a fridge)
    /// </summary>
    Liquid,
    
    /// <summary>
    /// Yeast is provided in a dried form, in a small package.
    /// Generally contains much more cells than in the liquid form, but are harder to rehydrate without damage.
    /// There are techniques to avoid killing half of the cell population when using them.
    /// Usually much cheaper packaging as well.
    /// </summary>
    Dry,
    
    /// <summary>
    /// Yeast cells can either be provided in one form or in the other.
    /// It depends on the manufacturer, nothing prevents them to do so.
    /// Except maybe production/packaging costs (...)
    /// </summary>
    Both,
    
    /// <summary>
    /// Yeast can be used either for its Bittering or Aromatic properties
    /// </summary>
    Unknown
}