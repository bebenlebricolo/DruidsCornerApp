using CommunityToolkit.Mvvm.ComponentModel;

namespace DruidsCornerApp.Models.MainContext;

/// <summary>
/// Compact Recipe that allows the recipe browser to list recipes with minimal data
/// </summary>
public partial class CompactHopModel : ObservableObject
{
    //public event Notify FavoriteToggled;
    
    /// <summary>
    /// Recipe's Alcoholic content
    /// </summary>
    [ObservableProperty]
    private string _name  = "";

    /// <summary>
    /// Hop alpha acid content range
    /// </summary>
    [ObservableProperty]
    private string _alphaAcids = "";
    
    /// <summary>
    /// Recipe's bitterness grade
    /// </summary>
    [ObservableProperty]
    private string _purpose  = "Aromatic";

    /// <summary>
    /// States the amount of this hop that we currently have in stock
    /// </summary>
    [ObservableProperty]
    private double _stockedAmount  = 153;

    /// <summary>
    /// States the global rating of this resource amongst the community
    /// </summary>
    [ObservableProperty]
    private double _rating  = 2.5;

    /// <summary>
    /// States whether this resource was set as a favorite by the user or not
    /// </summary>
    [ObservableProperty]
    private bool _favorite = false;

    /// <summary>
    /// List of tags for the selected Hop variety
    /// </summary>
    [ObservableProperty]
    private List<string> _tags  = new();
}