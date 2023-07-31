namespace DruidsCornerApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "Default title";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    private bool _isLoading = false;

    public bool IsNotLoading => !IsLoading;

    public BaseViewModel()
    {
    }
    
    public BaseViewModel(string title, bool isLoading)
    {
        _title = title;
        _isLoading = isLoading;
    }
    
}

