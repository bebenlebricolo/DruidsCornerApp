namespace DruidsCornerApp.Models.Google;

/// <summary>
/// Google Account DTO object
/// </summary>
public record GoogleAccount
{
    /// <summary>
    /// User email address
    /// </summary>
    public string Email { get; set; } = "";
    
    /// <summary>
    /// User unique Id
    /// </summary>
    public string Id { get; set; } = "";
    
    /// <summary>
    /// User full name
    /// </summary>
    public string FullName { get; set; } = "";
    
    /// <summary>
    /// Username / alias
    /// </summary>
    public string UserName { get; set; } = "";
}