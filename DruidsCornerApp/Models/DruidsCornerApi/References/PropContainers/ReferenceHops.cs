using DruidsCornerApp.Models.DruidsCornerApi.References.Properties;

namespace DruidsCornerApp.Models.DruidsCornerApi.References.PropContainers
{
    /// <summary>
    /// Very simple Known Good Hops list
    /// </summary>
    public class ReferenceHops
    {
        /// <summary>
        /// List of Hop properties
        /// </summary>
        public List<HopProperty> Hops {get; set; }= new List<HopProperty>();  
    }
}