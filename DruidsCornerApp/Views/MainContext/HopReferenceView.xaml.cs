using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApp.Models.MainContext;

namespace DruidsCornerApp.Views.MainContext;

public partial class HopReferenceView : ContentView
{
    public ObservableCollection<CompactHopModel> Hops { get; } = new()
    {
        new CompactHopModel
        {
            Name = "Magnum US",
            Purpose = "Bittering",
            Rating = 3.5,
            AlphaAcids = "10 - 16 %",
            Favorite = false,
            StockedAmount = 153
        }
    };

    public HopReferenceView()
    {
        InitializeComponent();
        InitFakeHops();
    }

    private void InitFakeHops()
    {
        Hops.Clear();
        Hops.Add(new CompactHopModel
        {
            Name = "Magnum US",
            Purpose = "Bittering",
            Rating = 3.5,
            AlphaAcids = "10 - 16 %",
            Favorite = false,
            StockedAmount = 153
        });
        Hops.Add(new CompactHopModel
        {
            Name = "Magnum GR",
            Purpose = "Bittering",
            Rating = 3.7,
            AlphaAcids = "12 - 16 %",
            Favorite = true,
            StockedAmount = 0
        });
        Hops.Add(new CompactHopModel
        {
            Name = "Cascade",
            Purpose = "Hybrid",
            Rating = 3.8,
            AlphaAcids = "4.5 - 9 %",
            Favorite = true,
            StockedAmount = 86
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Chinook",
            Purpose = "Hybrid",
            Rating = 3.8,
            AlphaAcids = "11.5 - 15 %",
            Favorite = true,
            StockedAmount = 33
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Saaz",
            Purpose = "Aromatic",
            Rating = 3.2,
            AlphaAcids = "2 - 5 %",
            Favorite = false,
            StockedAmount = 0
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Hallertau Blanc",
            Purpose = "Aromatic",
            Rating = 4.1,
            AlphaAcids = "9 - 12 %",
            Favorite = true,
            StockedAmount = 0
        });
    }
}