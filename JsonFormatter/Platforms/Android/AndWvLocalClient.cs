using Android.Content;
using Android.Webkit;
using AndroidX.Annotations;
using AndroidX.WebKit;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Telephony.CarrierConfigManager;

namespace JsonFormatter.Platforms.LAndroid;

internal class AndWvLocalClient : WebViewClient
{
    readonly WebViewAssetLoader loader;

    public AndWvLocalClient(Context ctx)
    {
        this.loader = new WebViewAssetLoader.Builder()
            .SetDomain("webapp")
            .AddPathHandler("/assets/", new WebViewAssetLoader.AssetsPathHandler(ctx))
            .Build();
    }

    [Override]
    public override WebResourceResponse? ShouldInterceptRequest(Android.Webkit.WebView? view, IWebResourceRequest? request)
    {
        var url = request?.Url;
        if (url is null) { return null; }

        Debug.WriteLine(url);

        var result = this.loader.ShouldInterceptRequest(request!.Url!);
        return result;
    }

}