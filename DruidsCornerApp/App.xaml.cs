using System.Text;
using DruidsCornerApp.Models;
using DruidsCornerApp.Services;
using Microsoft.Extensions.Configuration;

namespace DruidsCornerApp;

public partial class App : Application
{
    // private readonly ConfigurationProvider _configurationProvider;
    private readonly ISecureStorageService _storageService;
    
    public App(ISecureStorageService secureStorageService)
    {
        // _configurationProvider = configurationProvider;
        _storageService = secureStorageService;
        InitializeComponent();
        MainPage = new BootShell();
    }
    
    protected override void OnStart()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var firstStart = Task.Run(async () => await _storageService.GetAsync(GlobalApplicationStates.AppFirstBootKey)).Result;
        
            // If the app already booted and initialization steps were performed correctly
            // We can directly skip to the application's core page
            if(firstStart != null  && !bool.Parse(firstStart))
            {
                MainPage = new AppShell(_storageService);
            }
        });

        base.OnStart();
    }
}