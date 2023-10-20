using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DruidsCornerApp.ViewModels.References;

namespace DruidsCornerApp.Views.References;

public partial class HopPage : ContentPage
{
    public HopPage(HopPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}