namespace DruidsCornerApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnStartBrowsingClicked(object? sender, EventArgs e)
	{
		StartBrowsingBtn.Text = "Clicked !";
	}
}

