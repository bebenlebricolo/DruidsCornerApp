using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.References;

namespace DruidsCornerApp.Models.MainContext;

/// <summary>
/// Compact Recipe that allows the recipe browser to list recipes with minimal data
/// </summary>
public partial class CompactHopModel : ObservableObject
{
    /// <summary>
    /// Hop id, refers to the real hop id in the
    /// database collection.
    /// This property is not displayed on screen and is there as a key for the HopPage/HopPageViewModel in order
    /// to display the appropriate hop when navigated to.
    /// </summary>
    [ObservableProperty]
    private string _id = "";
    
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

public static class CompactHopModelHelper
{
    /// <summary>
    /// Converts a full hop model into this smaller and compact version.
    /// Note that some fields are not filled, as data source is coming from somewhere else.
    /// For instance, each user has a list of favorite elements, and this is not encoded in the full HopModel, as this
    /// is a personal and private data resource. It needs to reside elsewhere.
    /// Same goes for the stocked amount.
    /// </summary>
    /// <param name="fullModel"></param>
    /// <returns></returns>
    public static CompactHopModel FromFullModel(HopModel fullModel)
    {
        var compact = new CompactHopModel()
        {
            Name = fullModel.Name,
            Purpose = fullModel.Purpose.ToString(),
            Rating = fullModel.Rating,
            Tags = fullModel.Tags,
            Id = fullModel.Id,
        };
        
        // Those fields are not always filled in the original database !
        if (fullModel.AlphaAcids != null)
        {
            compact.AlphaAcids = $"{fullModel.AlphaAcids.Min} - {fullModel.AlphaAcids.Max}";
        }
        
        return compact;
    }
}