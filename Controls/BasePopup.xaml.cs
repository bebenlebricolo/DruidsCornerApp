using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DruidsCornerApp.Controls;

public partial class BasePopup : Popup
{
    private IView? _centralElement = null;
    
    /// <summary>
    /// Standard constructor
    /// </summary>
    public BasePopup()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Sets the popup title
    /// </summary>
    /// <returns></returns>
    public void SetTitle(string text)
    {
        TitleLabel.Text = text;
    }

    /// <summary>
    /// Sets the popup message
    /// </summary>
    /// <returns></returns>
    public void SetMessage(string text)
    {
        MessageLabel.Text = text;
    }

    /// <summary>
    /// Sets the central element of the popup (could be anything, an image, an ActivityIndicator, etc...)
    /// </summary>
    /// <param name="control"></param>
    public void SetCentralElement(IView control)
    {
        if (_centralElement == null)
        {
            int index = CentralLayout.IndexOf(SeparatorControl);
            CentralLayout.Insert(index + 1, control);
        }
        else
        {
            int index = CentralLayout.IndexOf(_centralElement);
            CentralLayout[index] = control;
        }
        _centralElement = control;
    }

    /// <summary>
    /// Remove element from central layout
    /// </summary>
    public void RemoveCentralElement()
    {
        if (_centralElement != null)
        {
            CentralLayout.Remove(_centralElement);
        }
    }
    
    /// <summary>
    /// Returns the Popup frame for further customization
    /// </summary>
    /// <returns></returns>
    public Label GetTitleLabel()
    {
        return TitleLabel;
    }

    /// <summary>
    /// Returns the Popup frame for further customization
    /// </summary>
    /// <returns></returns>
    public Label GetMessageLabel()
    {
        return MessageLabel;
    }


    /// <summary>
    /// Returns the Popup frame for further customization
    /// </summary>
    /// <returns></returns>
    public Frame GetFrame()
    {
        return OuterFrame;
    }

    /// <summary>
    /// Returns the button stack layout for further customization
    /// </summary>
    /// <returns></returns>
    public Grid GetButtonsGridLayout()
    {
        return ButtonGridLayout;
    }

    public void SetButtonVisibilityStatus(bool visible)
    {
        OkButton.IsVisible = visible;
    }

    public void SetButtonEnable(bool enable)
    {
        OkButton.IsEnabled = enable;
    }

    public Button GetOkButton()
    {
        return OkButton;
    }

    public void UpdateLayout()
    {
        
    }
    
    protected void OkButton_OnClicked(object? sender, EventArgs e)
    {
        Close();
    }
}