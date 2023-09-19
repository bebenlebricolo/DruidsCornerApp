using DruidsCornerApp.Models.DruidsCornerApi.References.Properties;

namespace DruidsCornerApp.Models.DruidsCornerApi.References.PropContainers
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