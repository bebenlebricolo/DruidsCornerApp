namespace DruidsCornerApiClient.Models.IndexedDb
{
    /// <summary>
    /// Encodes basic information about BrewDog's beer recipe.
    /// </summary>
    public class IndexedFoodPairingDb : IndexedDb
    {
        /// <summary>
        /// List of properties in a reversed DB construct
        /// </summary>
        public List<ReversedPropMapping> FoodPairing  {get; set;} = new List<ReversedPropMapping>();
    }
}
