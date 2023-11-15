namespace DruidsCornerApp.Models;

public static class GlobalApplicationStates
{
    /// <summary>
    /// Allows to cache whether the app has never booted on this device or not
    /// Moreover, it's used to know at startup time whether the user needs to see the introduction pages / first login screen or
    /// if we can directly skip this to go to the application's core.
    /// </summary>
    public static string AppFirstBootKey = "APPLICATION_FIRST_BOOT";

}