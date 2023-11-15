namespace DruidsCornerApiClient.Models.References.Properties
{
    /// <summary>
    /// Encodes a known good Hop property
    /// This is the base for all ReferenceProperties (aka "known good" properties in the DiyDogExtractor databases)
    /// </summary>
    public class HopProperty : BaseProperty
    {
        /// <summary>
        /// List of potential aliases for this property
        /// </summary>
        public List<string>? Aliases { get; set; } = null;
    }
}