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
            var imageFrame = new Frame()
            {
                Content = new Image()
                {
                    Source = ImageSource.FromUri(new Uri(e.account.PhotoUrl!)),
                    HeightRequest = 80,
                    WidthRequest = 80
                },
                CornerRadius = 15,
                Padding = new Thickness(0,0),
                Margin = new Thickness(0,0),
                IsClippedToBounds = true,
                Background = null,
                BorderColor = Colors.Transparent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
            };

            var nameLabel = new Label()
            {
                Text = e.account.FullName,
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center
            };

            var emailLabel = new Label()
            {
                Text = e.account.Email,
                FontSize = 10,
                VerticalOptions = LayoutOptions.Center
            };

            var labelStackLayout = new VerticalStackLayout()
            {
                Children =
                {
                    nameLabel,
                    emailLabel
                },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            var grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection(new[]
                                                                   {
                                                                       new ColumnDefinition(GridLength.Auto),
                                                                       new ColumnDefinition(GridLength.Star)
                                                                   }
                                                                  ),
                ColumnSpacing = 30,
                Margin = new Thickness(0,0),
                Padding = new Thickness(0,0)
            };
            
            // Adding the elements inside
            grid.Add(imageFrame, 0,0);
            grid.Add(labelStackLayout, 1,0);
            
            // Put everyone into a nice frame
            var buttonFrame = new Frame()
            {
                CornerRadius = 10,
                BackgroundColor = Colors.White,
                BorderColor = Colors.DeepSkyBlue,
                Content = grid,
                Margin = new Thickness(0,0),
                Padding = new Thickness(10,10)
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