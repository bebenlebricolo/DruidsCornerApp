namespace DruidsCornerApp.Models.DruidsCornerApi.IndexedDb
{
    /// <summary>
    /// Encodes basic information about BrewDog's beer recipe.
    /// </summary>
    public class IndexedHopDb : IndexedDb
    {
        /// <summary>
        /// List of properties in a reversed DB construct
        /// </summary>
        public List<ReversedPropMapping> Hops {get; set;} = new List<ReversedPropMapping>();  
    }
}
