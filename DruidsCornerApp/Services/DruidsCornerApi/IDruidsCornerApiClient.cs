using DruidsCornerAPI.Models.DiyDog.RecipeDb;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public interface IDruidsCornerApiClient
{
    /// <summary>
    /// Retrieves a single recipe
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public Task<Recipe> GetRecipeAsync(int number);

    /// <summary>
    /// Lists all available recipes from database
    /// </summary>
    /// <returns>List of available recipes</returns>
    public Task<List<Recipe>> GetAllRecipesAsync();
}