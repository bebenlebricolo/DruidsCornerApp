using DruidsCornerAPI.Models.DiyDog.RecipeDb;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public interface IDruidsCornerApiClient
{
    public Task<Recipe> GetRecipeAsync(int number);
}