using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class GoogleSignInPage : ContentPage
{
    private readonly GoogleSignInPageViewModel _viewModel;

    public GoogleSignInPage(GoogleSignInPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public void StatusUpdateHandler(object sender, GoogleSignInEventArgs e)
    {
        var toto = sender.GetType();
        // do something here
        if (e.account != null)
        {
            var buttonFrame = new Frame()
            {
                CornerRadius = 20,
                BackgroundColor = Colors.White,
                BorderColor = Colors.DeepSkyBlue,
                Content = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection(new[]
                                                                       {
                                                                           new ColumnDefinition(GridLength.Auto),
                                                                           new ColumnDefinition(GridLength.Star)
                                                                       }
                                                                      ),
                    ColumnSpacing = 10,
                    Children =
                    {
                        // Top left
                        new Frame()
                        {
                            Content = new Image()
                            {
                                Source = ImageSource.FromUri(new Uri(e.account.PhotoUrl!))
                            },
                            CornerRadius = 30,
                            IsClippedToBounds = true,
                            Background = null,
                            BorderColor = Colors.Transparent,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Start
                        },
                        new VerticalStackLayout()
                        {
                            Children =
                            {
                                new Label()
                                {
                                    Text = e.account.FullName,
                                    FontSize = 20,
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label()
                                {
                                    Text = e.account.Email,
                                    FontSize = 10,
                                    VerticalOptions = LayoutOptions.Center
                                }
                            },
                            HorizontalOptions = LayoutOptions.Fill
                        }
                    }
                }
            };

            BaseVerticalStackLayout.Add(buttonFrame);
        }


        // Unregister
        _viewModel.OnSignInFinished -= StatusUpdateHandler;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.StartPopulatingUi();

        // Register event
        _viewModel.OnSignInFinished += StatusUpdateHandler;
    }
}