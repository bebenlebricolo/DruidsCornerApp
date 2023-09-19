using DruidsCornerApp.Models.DruidsCornerApi.References.Properties;

namespace DruidsCornerApp.Models.DruidsCornerApi.References.PropContainers
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