using DruidsCornerApp.ViewModels.MainContext;

namespace DruidsCornerApp.Views.MainContext;

public partial class HopReferenceView : ContentView
{
    private readonly HopReferenceViewModel _viewModel;
    
    public HopReferenceView()
    {
        InitializeComponent();
        
    }
    
    public HopReferenceView(HopReferenceViewModel viewModel) : this()
    {
        BindingContext = viewModel;
    }
}