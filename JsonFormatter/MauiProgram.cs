using Android.App;
using Android.Content;
using Microsoft.Maui.LifecycleEvents;
using System.Runtime.CompilerServices;

namespace JsonFormatter;

public static class MauiProgram
{
	public record ActivityResult(Result resultCode, Intent? data);

	const int FileRequestCode = 100;
	static TaskCompletionSource<ActivityResult>? fileTcs;

	public static async Task<ActivityResult> RequestAndroidFileAsync(Intent intent)
	{
#if !ANDROID
		throw new PlatformNotSupportedException("For Android only");
#endif

		fileTcs = new TaskCompletionSource<ActivityResult>();
		Platform.CurrentActivity!.StartActivityForResult(intent, FileRequestCode);

		return await fileTcs.Task;
	}

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
			});

		return builder.Build();
	}
}
