namespace DruidsCornerApp.Models.DruidsCornerApi.References.Properties
{
    /// <summary>
    /// Encodes a known good Malt property
    /// This is the base for all ReferenceProperties (aka "known good" properties in the DiyDogExtractor databases)
    /// </summary>
    public class MaltProperty : BaseProperty
    {
        /// <summary>
        /// Manufacturer identifier
        /// </summary>
        public string? Manufacturer { get; set; } = null;

        /// <summary>
        /// List of potential aliases for this property
        /// </summary>
        public List<string>? Aliases { get; set; } = null;
    }
}