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

        // coordinates of the address
        var latitude = 54.9130415;
        var longitude = 9.77896211;

        // creates a Location object using the coordinates
        var location = new Location(latitude, longitude);

        // creates a MapSpan centered on the Location with a 1 km radius
        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1));

        // moves the map view to the defined region
        HestiaMaps.MoveToRegion(mapSpan);

        // Sets coordinates in the ViewModel
        if (BindingContext is AddDeviceViewModel vm)
        {
            vm.LatitudeMaps = (float)latitude;
            vm.LongitudeMaps = (float)longitude;
        }
    }

}
