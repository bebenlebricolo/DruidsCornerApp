using DruidsCornerApp.ViewModels.Recipes;

namespace DruidsCornerApp.Views.MainContext;

public partial class RecipeExplorerPage : ContentPage
{
    public RecipeExplorerPage()
    {
        InitializeComponent();
    }
    
    public RecipeExplorerPage(RecipeExplorerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}