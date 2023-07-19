using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DruidsCornerApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void ImageButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", animate:true);
    }
}