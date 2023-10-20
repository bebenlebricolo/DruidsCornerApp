namespace DruidsCornerApp.Models.References;

/// <summary>
/// Encodes simple numeric range concept
/// </summary>
public record NumericRange
{
    /// <summary>
    /// Lower bound of the range
    /// </summary>
    public double Min { get; set; }
    
    /// <summary>
    /// Higher bound of the range
    /// </summary>
    public double Max { get; set; }
}