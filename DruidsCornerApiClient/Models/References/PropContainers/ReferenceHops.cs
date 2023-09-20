using DruidsCornerApiClient.Models.References.Properties;

namespace DruidsCornerApiClient.Models.References.PropContainers
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