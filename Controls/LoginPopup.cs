namespace DruidsCornerApp.Controls;

public class LoginPopup : BasePopup
{
    private ActivityIndicator? _activityIndicator = null;
    
    public LoginPopup(bool withActivity = true)
    {
        if (withActivity)
        {
            _activityIndicator = new ActivityIndicator()
            {
                Color = Colors.SkyBlue
            };
            SetCentralElement(_activityIndicator);
        }
    }

    public ActivityIndicator? GetActivityIndicator()
    {
        return _activityIndicator;
    }
}