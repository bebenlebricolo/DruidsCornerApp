namespace DruidsCornerAPI.Models.DiyDog.RecipeDb
{
    /// <summary>
    /// Temperature instruction for Mash
    /// </summary>
    public class Temperature
    {
        /// <summary>
        /// Temperature expressed in Celsius degrees
        /// </summary>
        public float Celsius { get; set; } = 0.0f;

        /// <summary>
        /// Temperature expressed using Fahrenheit degrees
        /// </summary>
        public float Fahrenheit { get; set; } = 0.0f;
    }

    /// <summary>
    /// Mash temperature
    /// </summary>
    public class MashTemp : Temperature
    {
        /// <summary>
        /// Time of the mash
        /// </summary>
        public float Time { get; set; } = 0.0f;
    }
}
