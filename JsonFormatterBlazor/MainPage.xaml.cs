using System.Diagnostics;

namespace JsonFormatterBlazor;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        this.wv.UrlLoading += this.Wv_UrlLoading;
    }

    private void Wv_UrlLoading(object? sender, Microsoft.AspNetCore.Components.WebView.UrlLoadingEventArgs e)
    {
        Debug.WriteLine(e.Url.ToString());
    }
}
