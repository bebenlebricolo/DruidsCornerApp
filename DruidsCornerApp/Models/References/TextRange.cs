namespace DruidsCornerApp.Models.References;

/// <summary>
/// That's a weird concept for a range, but it is useful for numerical data which was
/// parsed as text earlier in the data extraction process, such as ratio ranges :
/// min = 2:3
/// max = 4:3
/// As long as we just use those data for displaying purposes, we should be fine with this naive implementation
/// </summary>
public record TextRange
{
    /// <summary>
    /// Lower bound of the range
    /// </summary>
    public string Min { get; set; } = "";

    /// <summary>
    /// Higher bound of the range
    /// </summary>
    public string Max { get; set; } = "";
}