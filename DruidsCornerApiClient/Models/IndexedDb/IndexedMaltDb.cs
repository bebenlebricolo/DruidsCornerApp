
namespace DruidsCornerApiClient.Models.IndexedDb
{
    /// <summary>
    /// Encodes basic information about BrewDog's beer recipe.
    /// </summary>
    public class IndexedMaltDb : IndexedDb
    {
        /// <summary>
        /// List of properties in a reversed DB construct
        /// </summary>
        public List<ReversedPropMapping> Malts {get; set;} = new List<ReversedPropMapping>();  
    }
}
