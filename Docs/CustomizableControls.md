# Customizable controls in .net maui and what I learned about them
Customizable controls can be created using base ContentView class :

Resources : 
* [Issue regarding one-way properties binding](https://github.com/dotnet/maui/issues/8501#issuecomment-1352834061)
* [Self referencing bindings (StackOverflow)](https://stackoverflow.com/a/72342037/8716917)
* [Custom controls in Xamarin Forms](https://www.mfractor.com/blogs/news/custom-controls-in-xamarin-forms)
* [Programming With Chris - Reusable, Custom Controls in .Net Maui](https://www.youtube.com/watch?v=YLx2L7SXeaY)
* [Microsoft Docs : custom controls with handlers](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/handlers/create)
  * This one is nice but goes in the details for platform-dependent handlers and implementations (which is really nice) but not really what 
we want to aim for small reusable controls  

### XAML file : 
First, the xaml file of the custom component ()
```xaml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
                 
             <!-- Import the local namespace as "local" or "unameit" -->         
             xmlns:local="clr-namespace:DruidsCornerApp.Controls.Entries"
             <!-- Import the class symbol that this xaml file is linked to --->
             x:Class="DruidsCornerApp.Controls.Entries.OutlinedEntry"
             
             <!-- Link in its datatype (used for self-binding) -->
             x:DataType="local:OutlinedEntry"
             
             <!-- Give it a proper name so that we can use self-referenced bindings -->
             x:Name="CustomControl">
               
                 <!-- Give it some content -->
                 <!-- The following line means : Bind this value to the Text property of the CustomControl class found in the *.xaml.cs implementation -->
                 <Label Text="{Binding Text, Source={x:Reference CustomControl}}"/>
               ...
 </ContentView>  
```

### CS implementation

```csharp
  public partial class OutlinedEntry : ContentView
  {  
      // First, add a new public property with custom getters and setters
      // Note that the get triggers the OnPropertyChanged() event when a new value is injected
      public string Text
      {
          get => (string)GetValue(TextProperty);
          set
          {
              SetValue(TextProperty, value);
              OnPropertyChanged();
          }
      }
      
      // Create a new static BindableProperty that maps to the XAML part of the custom control interface
      // (So that we can do <CustomControl Text="blahblahblah" ... /> where "Text" is the custom static bindable property we are creating right below
      // > Note that we are using the TwoWay Binding mode. Normally it'll default to a single Way binding mode, where programmatically changing the property
      // Will reflect in the the UI, but not the reverse. This is fine for display only controls, but not for entry fields !
      public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                        typeof(string),
                                                                        typeof(OutlinedEntry),
                                                                        defaultBindingMode:BindingMode.TwoWay, // Here, the Directrion is important !
                                                                        propertyChanged: OnTextPropertyChanged  // Custom event that'll handle static xaml data forwarding to the actual Property of this instance
                                                                       );
      
      // Custom event that handles the static xaml data generated when building the codebase.
      // It's a custom static handler for a given BindableObject (this instance) and it's used to synchronise instance's properties with the xaml datamodel.
      private static void OnTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
      {
          var control = (OutlinedEntry)bindable;
          control.Text = (string)newvalue;
          control.EntryText.Text = control.Text;
          control.InvalidateLayout();
      }
  }
```

### Use in xaml and binding in a page with the page's ViewModel

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DruidsCornerApp.Views.AccountCreationPage"
             xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
             xmlns:viewmodel="clr-namespace:DruidsCornerApp.ViewModels"
             xmlns:customEntries="clr-namespace:DruidsCornerApp.Controls.Entries"
             Shell.NavBarIsVisible="False"
             x:DataType="viewmodel:AccountCreationPageViewModel">
    <ContentPage.Content>
        <VerticalStackLayout Padding="0,10" Spacing="10">
                            <Label Text="Display name" TextColor="#304057" />
                            <customEntries:OutlinedEntry x:Name="NameEntry"
                                                         BackgroundColor="Beige"
                                                         BorderColor="#aaff6a00" <!-- Statically forwarded to the custom control instance, set once-->
                                                         
                                                         <!-- This property binds with the underlying AccountCreationPageViewModel, two ways data binding -->
                                                         Text="{Binding DisplayName}"
                                                         
                                                         TextColor="#ff6a00" 
                                                         Icon="mail.svg"
                                                         CornerRadius="25"
                                                         Placeholder="my name here"
                                                         PlaceholderColor="#44808080">
```

### Content page CSharp binding
```csharp
using DruidsCornerApp.ViewModels;
namespace DruidsCornerApp.Views;

public partial class AccountCreationPage : ContentPage
{
    
    public AccountCreationPage(AccountCreationPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // ViewModel binding 
    }
    
    // ...
}
```