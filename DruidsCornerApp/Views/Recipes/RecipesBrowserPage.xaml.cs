using DruidsCornerApp.ViewModels.Recipes;

namespace DruidsCornerApp.Views.Recipes;

public partial class RecipesBrowserPage : ContentPage
{
    public RecipesBrowserPage(RecipesBrowserPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}