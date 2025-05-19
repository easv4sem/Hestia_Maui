namespace Hestia_Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
#if ANDROID || IOS
			.UseMauiMaps()
#elif WINDOWS
			// Add your API key here.
			// See also https://github.com/CommunityToolkit/Maui/discussions/2123
			.UseMauiCommunityToolkitMaps("YOUR_API_KEY")
#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialSymbol");
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<UrlOrganisationViewModel>();

		builder.Services.AddSingleton<SignInViewModel>();

		builder.Services.AddSingleton<HomeViewModel>();

		builder.Services.AddSingleton<DevicesViewModel>();

		builder.Services.AddSingleton<AddDeviceViewModel>();

		builder.Services.AddSingleton<MapViewModel>();

		return builder.Build();
	}
}
