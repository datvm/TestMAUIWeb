
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using System.Runtime.CompilerServices;

#if ANDROID
using Android.App;
using Android.Content;
using JsonFormatter.Platforms.LAndroid;
#endif

namespace JsonFormatter;

public static class MauiProgram
{

#if ANDROID
    public record ActivityResult(Result resultCode, Intent? data);
    const int FileRequestCode = 100;
    static TaskCompletionSource<ActivityResult>? fileTcs;


    public static async Task<ActivityResult> RequestAndroidFileAsync(Intent intent)
    {
        fileTcs = new TaskCompletionSource<ActivityResult>();
        Platform.CurrentActivity!.StartActivityForResult(intent, FileRequestCode);

        return await fileTcs.Task;
    }
#endif

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLifecycleEvents(e =>
            {
#if ANDROID
                e.AddAndroid(droid =>
                {
                    droid.OnActivityResult((activity, requestCode, resultCode, data) =>
                    {
                        if (requestCode == FileRequestCode)
                        {
                            var tcs = fileTcs;

                            if (tcs is not null)
                            {
                                fileTcs = null;
                                tcs?.TrySetResult(new(resultCode, data));
                            }

                        }
                    });
                });
#endif
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler<WebView, LocalAndroidWvHandler>();
#endif
            });

        return builder.Build();
    }
}

#if ANDROID
class LocalAndroidWvHandler : WebViewHandler
{

    protected override Android.Webkit.WebView CreatePlatformView()
    {
        var ctx = Android.App.Application.Context;
        var wv = new Android.Webkit.WebView(ctx);
        wv.SetWebViewClient(new AndWvLocalClient(ctx));

        wv.LoadUrl("https://www.google.com/assets/index.html");

        return wv;
    }

}
#endif