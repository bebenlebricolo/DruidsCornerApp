using DruidsCornerAPI.Models.DiyDog.RecipeDb;
using DruidsCornerApp.Models.DruidsCornerApi.Wrappers;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public interface IDruidsCornerApiClient
{
    /// <summary>
    /// Retrieves a single recipe
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public Task<Recipe?> GetRecipeByNumberAsync(uint number);

    /// <summary>
    /// Lists all available recipes from database
    /// </summary>
    /// <returns>List of available recipes</returns>
    public Task<List<Recipe>?> GetAllRecipesAsync();

    /// <summary>
    /// Retrieves a recipe using its name.
    /// This endpoint uses FuzzySearch algorithms in order to find the closest name out of them all
    /// and returns the recipe that has the best matching score.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<RecipeResult?> GetRecipeByName(string name);
}