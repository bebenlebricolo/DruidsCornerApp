using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.ViewModels.MainContext;

namespace DruidsCornerApp.Controls.MainContext;

public partial class HopCardView : ContentView
{
    private readonly CompactHopModel? _hopModel;

    // public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data),
    //                                                                                typeof(object),
    //                                                                                typeof(CompactHopModel),
    //                                                                                null,
    //                                                                                propertyChanged: DataChanged);
    //
    // public CompactHopModel Data
    // {
    //     get => (CompactHopModel)GetValue(DataProperty);
    //     set => SetValue(DataProperty, value);
    // }
    //
    // private static void DataChanged(BindableObject bindable, object oldValue, object newValue)
    // {
    //     // if (bindable is CompactHopModel contentPresenter)
    //     // {
    //     //     BindableLayout.SetItemsSource(contentPresenter.HostGrid, new object[] { newValue });
    //     // }
    // }

    public HopCardView()
    {
        InitializeComponent();

        // Should always be created with the adequate
        //_hopModel = (BindingContext as CompactHopModel);
        //_hopModel = Data;
        
        // Binding context is forwarded within the CollectionView
        // if (_hopModel != null)
        // {
        //     _hopModel.PropertyChanged += HopModelFavoritePropertyChanged;
        //     HandleHopFavorite();
        // }
    }


    private void HandleHopFavorite()
    {
        
    }

    private void HopModelFavoritePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CompactHopModel.Favorite))
        {
            HandleHopFavorite();
        }
    }
    
}