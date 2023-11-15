namespace DruidsCornerApp.Models.Login;

/// <summary>
/// Encodes the various states of user account connection, upon application start.
/// It is used in order to know how to handle connection states
/// </summary>
public enum AccountStates
{
    /// <summary>
    /// Connected mode is used when the user has first logged in the application (created an account and Signed In)
    /// </summary>
    BasicCredsConnection,

    /// <summary>
    /// Used to record whenever the user decided to go for the GoogleAccount link mode
    /// </summary>
    GoogleIdConnection,

    /// <summary>
    /// When the user declined the account creation step and went for the "Guest mode"
    /// </summary>
    GuestMode,
    
    /// <summary>
    /// Default mode, used when the app starts for the first time, without
    /// Knowing if the user wants to connect its account in the app or not. 
    /// </summary>
    Unknown
}

/// <summary>
/// Small static class used to read and write values to and from SecureStorage
/// </summary>
public static class AccountKeys
{
    public static string TokenKey = "idToken";
    public static string PasswordKey = "password";
    public static string EmailKey = "email";
    public static string AccountStateKey = "accountState";
}