using Microsoft.Maui.Maps;

namespace Hestia_Maui.Views;

public partial class AddDevicePage : ContentPage
{
	public AddDevicePage(AddDeviceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Adressens koordinater (fra tidligere)
        var latitude = 54.9130415;
        var longitude = 9.77896211;

        // Opret en Location baseret på de givne koordinater
        var location = new Location(latitude, longitude);

        // Opret et MapSpan centreret på denne Location
        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1));

        // Opdater kortet med den nye region
        HestiaMaps.MoveToRegion(mapSpan);
    }

}
