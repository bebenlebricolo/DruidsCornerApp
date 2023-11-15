using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Models.Wrappers;

namespace DruidsCornerApiClient.Services.Interfaces;

public interface IRecipeClient
{
    /// <summary>
    /// Retrieves a single recipe
    /// </summary>
    /// <param name="number"></param>
    /// <param name="token">JWT Token used to authenticate</param>
    /// <returns></returns>
    public Task<Recipe?> GetRecipeByNumberAsync(uint number, string token);

    /// <summary>
    /// Lists all available recipes from database
    /// </summary>
    /// <param name="token">JWT Token used to authenticate</param>
    /// <returns>List of available recipes</returns>
    public Task<List<Recipe>?> GetAllRecipesAsync(string token);

    /// <summary>
    /// Retrieves a recipe using its name.
    /// This endpoint uses FuzzySearch algorithms in order to find the closest name out of them all
    /// and returns the recipe that has the best matching score.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="token">JWT Token used to authenticate</param>
    /// <returns></returns>
    public Task<RecipeResult?> GetRecipeByNameAsync(string name, string token);
}