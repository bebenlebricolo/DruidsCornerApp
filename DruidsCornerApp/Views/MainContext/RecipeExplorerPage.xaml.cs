using RecipeExplorerViewModel = DruidsCornerApp.ViewModels.MainContext.RecipeExplorerViewModel;

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