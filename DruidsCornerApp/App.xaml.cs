using DruidsCornerApp.Utils;

namespace DruidsCornerApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

//     protected override void OnStart()
//     {
//         base.OnStart();
//         var shell = (AppShell) MainPage;
//         Task.Run(async() => await shell.GoToAsync(Navigator.GetWelcomePageRoute()));
//     }
}