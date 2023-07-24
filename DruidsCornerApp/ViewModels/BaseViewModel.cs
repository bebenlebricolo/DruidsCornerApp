namespace DruidsCornerApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    string title = "Default title";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    bool isLoading = false;

    public bool IsNotLoading => !IsLoading;
}

