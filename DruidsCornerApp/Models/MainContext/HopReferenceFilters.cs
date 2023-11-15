using DruidsCornerApiClient.Models.Search;

namespace DruidsCornerApp.Models.MainContext;

public enum HopAttributes
{
    /// <summary>
    /// Aromatic hop
    /// </summary>
    Aromatic,
    
    /// <summary>
    /// Bittering hop
    /// </summary>
    Bittering,
    
    /// <summary>
    /// Dual purpose hop, can be used as an Aromatic or Bittering one
    /// </summary>
    Hybrid
}

/// <summary>
/// Wraps Hop filters (used only by the view for UI/UX purposes, translated into proper HopQuery object in DruidsCornerApi Client)
/// </summary>
public class HopReferenceFilters
{
    /// <summary>
    /// Hop names queries
    /// </summary>
    public List<string> Names { get; set; } = new();

    /// <summary>
    /// Alpha acids range
    /// </summary>
    public Range<double> AlphaAcids { get; set; } = new Range<double>(0,0);
    
    /// <summary>
    ///  Beta Acids range
    /// </summary>
    public Range<double> BetaAcids { get; set; } = new Range<double>(0,0);
    
    /// <summary>
    /// List of countries of origin for hop cultures
    /// </summary>
    public List<string> CountryOfOrigin { get; set; } = new ();

    /// <summary>
    /// List of tags for hops, amongst all the ones we can find for those (floral, citrus, etc...)
    /// </summary>
    public List<string> TagList { get; set; } = new();

}