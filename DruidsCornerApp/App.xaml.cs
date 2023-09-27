using System.Text;
using DruidsCornerApp.Models;
using DruidsCornerApp.Services;

namespace DruidsCornerApp;

public partial class App : Application
{
    private readonly ISecureStorageService _storageService;
    
    public App(ISecureStorageService secureStorageService)
    {
        _storageService = secureStorageService;
        InitializeComponent();

        var firstStart = Task.Run(async () => await _storageService.GetAsync(GlobalApplicationStates.AppFirstBootKey)).Result;
            
        // If the app already booted and initialization steps were performed correctly
        // We can directly skip to the application's core page
        if(firstStart != null  && !bool.Parse(firstStart))
        {
            MainPage = new AppShell(_storageService);
        }
        else
        {
            MainPage = new BootShell();
        }
    }
    
//     protected override void OnStart()
//     {
//         base.OnStart();
//         var shell = (AppShell) MainPage;
//         Task.Run(async() => await shell.GoToAsync(Navigator.GetWelcomePageRoute()));
//     }
}