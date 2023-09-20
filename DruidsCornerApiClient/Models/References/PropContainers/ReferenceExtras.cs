using DruidsCornerApiClient.Models.References.Properties;

namespace DruidsCornerApiClient.Models.References.PropContainers
{
    /// <summary>
    /// Very simple Known Good Hops list
    /// </summary>
    public class ReferenceExtras
    {
        /// <summary>
        /// List of Hop properties
        /// </summary>
        public List<ExtraProperty> Extras {get; set; }= new List<ExtraProperty>();  
    }
}