using DruidsCornerApiClient.Models.References.Properties;

namespace DruidsCornerApiClient.Models.References.PropContainers
{
    /// <summary>
    /// Very simple Known Good Styles list
    /// </summary>
    public class ReferenceStyles
    {
        /// <summary>
        /// List of Style properties
        /// </summary>
        public List<StyleProperty> Styles {get; set; } = new List<StyleProperty>();  
    }
}