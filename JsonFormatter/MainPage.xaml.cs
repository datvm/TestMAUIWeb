using Android.Content;
using Android.Webkit;
using Java.Interop;
using Java.IO;
using Java.Net;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Text;

namespace JsonFormatter;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

		this.ModifyWebView();
    }

	void ModifyWebView()
	{
		WebViewHandler.Mapper.AppendToMapping("WebViewWithDownload", (handler, view) =>
		{
#if ANDROID
			var andView = handler.PlatformView;
			andView.SetDownloadListener(new DownloadListenerHandler());
#endif
		});
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

}

