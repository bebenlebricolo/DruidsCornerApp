using System.Text.RegularExpressions;

namespace DruidsCornerApp.Utils;

public static class CredentialsChecker
{
    private const string EmailPattern = """([\wa-zA-Z0-9.-]*)@(\w*).(\w*)""";
    /// <summary>
    /// Reads the current .apk application name 
    /// </summary>
    /// <returns></returns>
    public static bool CheckEmailFormatting(string email)
    {
        // Apply regex
        Regex rg = new Regex(EmailPattern);
        return rg.IsMatch(email);
    }
}