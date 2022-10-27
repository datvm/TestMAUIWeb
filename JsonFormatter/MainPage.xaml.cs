
using Microsoft.Maui.Handlers;
using System.Diagnostics;
using System.Text;

#if ANDROID
using Android.Content;
using Android.Runtime;
using Android.Webkit;
using AndroidX.Annotations;
using AndroidX.WebKit;
using Java.IO;
#endif

namespace JsonFormatter;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
#if ANDROID
        //this.ModifyWebView();
#endif

        InitializeComponent();

#if ANDROID
        //this.webView.Source = "https://webapp/assets/index.html";
        this.webView.Source = "https://www.google.com/assets/index.html";
#endif
    }

#if ANDROID
    void ModifyWebView()
    {
        //WebViewHandler.Mapper.AppendToMapping("MyAndroidWebView", (handler, view) =>
        //{
        //    var v = handler.PlatformView;
        //    v.SetDownloadListener(new DownloadListenerHandler());
        //});
    }

    

    class DownloadListenerHandler : Java.Lang.Object, IDownloadListener
    {
        public async void OnDownloadStart(string? url, string? userAgent, string? contentDisposition, string? mimetype, long contentLength)
        {
            // There should be URL parsing for base64 as well but for now just get it
            // For blob, extra parsing is needed
            var content = url!;

            var picker = new Intent(Intent.ActionCreateDocument);
            picker.AddCategory(Intent.CategoryOpenable);
            picker.SetType("application/json");
            picker.PutExtra(Intent.ExtraTitle, "file.json"); // Or change file name to whatever

            var (result, data) = await MauiProgram.RequestAndroidFileAsync(picker);
            var fileUri = data?.Data;
            if (result != Android.App.Result.Ok || fileUri is null)
            {
                // Handle error here
                return;
            }

            var file = Platform.CurrentActivity?.ContentResolver?.OpenFileDescriptor(fileUri, "w");
            if (file?.FileDescriptor is null)
            {
                // Something is wrong here
                return;
            }

            using var jOutStream = new FileOutputStream(file.FileDescriptor);
            var bytes = Encoding.UTF8.GetBytes(content);
            jOutStream.Write(bytes);
            await jOutStream.FlushAsync();
            jOutStream.Close();
        }
    }

#endif

}

