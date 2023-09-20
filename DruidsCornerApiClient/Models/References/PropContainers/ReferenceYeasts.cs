using DruidsCornerApiClient.Models.References.Properties;

namespace DruidsCornerApiClient.Models.References.PropContainers
{
    /// <summary>
    /// Very simple Known Good Yeasts list
    /// </summary>
    public class ReferenceYeasts
    {
        /// <summary>
        /// List of yeast properties
        /// </summary>
        public List<YeastProperty> Yeasts {get; set; } = new List<YeastProperty>();  
    }
}