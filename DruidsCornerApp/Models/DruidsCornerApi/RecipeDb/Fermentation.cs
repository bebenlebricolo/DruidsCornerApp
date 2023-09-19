
namespace DruidsCornerApp.Models.DruidsCornerApi.RecipeDb
{
    /// <summary>
    /// Fermentation temperatures instructions
    /// </summary>
    public class Fermentation : Temperature
    {
        /// <summary>
        /// Optional time (used for extended fermentation steps, such as cask-aging)
        /// </summary>
        public float? Time { get; set; } = null;

        /// <summary>
        /// Optional additional tips indicated by the brewer's team
        /// </summary>
        public List<string> Tips { get; set; } = new List<string>();
    }
}
