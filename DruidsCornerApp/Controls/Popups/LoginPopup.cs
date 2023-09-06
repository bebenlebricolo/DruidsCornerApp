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

        // We have to use BasePopup as a bridge, because XAML generated properties are made private by default ...
        // Note : actually no ! we can tweak the field scopes using the x:FieldModifier="public|protected|private|internal|private" for custom fields.
        SetTitle("SignIn");
        SetMessage("Signin you In ...");
        SetButtonVisibilityStatus(false);
    }

    public ActivityIndicator? GetActivityIndicator()
    {
        return _activityIndicator;
    }
}