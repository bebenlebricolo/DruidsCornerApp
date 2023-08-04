namespace DruidsCornerAPI.Models.DiyDog.References
{
    /// <summary>
    /// Depicts the main kinds of extra ingredients found in a recipe
    /// </summary>
    public enum ExtraKind
    {
        /// <summary>
        /// Extra ingredient used in the Boil/Fermentation process (in DiyDog recipes, both are merged in the "Hops" category)
        /// </summary>
        Boil,

        /// <summary>
        /// Extra ingredient used in the Mash process
        /// </summary>
        Mash
    }
    
    /// <summary>
    /// Encodes a known good Hop property
    /// This is the base for all ReferenceProperties (aka "known good" properties in the DiyDogExtractor databases)
    /// </summary>
    public class ExtraProperty : BaseProperty
    {
        /// <summary>
        /// Extra ingredient kind, be it a boil/fermentation extra ingredient or a Mash ingredient
        /// </summary>
        /// <value></value>
        public ExtraKind Kind {get; set;} = ExtraKind.Boil;
    }
}