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

    private async void ForgottenPasswordButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Forgotten password ?", "Too bad !", "Ok");
    }

    private async void LoginButton_OnClicked(object? sender, EventArgs e)
    {
        var passwordFrame = PasswordFrame;
        passwordFrame.BorderColor = Colors.Red;
        PasswordEntry.Text = "";
        
        await Shell.Current.DisplayAlert("Login failed", "Invalid credentials", "Ok");
    }
}