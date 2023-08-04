namespace DruidsCornerAPI.Models.DiyDog.References
{
    /// <summary>
    /// Very simple Known Good Malts list
    /// </summary>
    public class ReferenceMalts
    {
        /// <summary>
        /// List of Malt properties
        /// </summary>
        public List<MaltProperty> Malts {get; set; } = new List<MaltProperty>();  
    }
}