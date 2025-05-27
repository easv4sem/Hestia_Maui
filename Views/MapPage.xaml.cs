using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Hestia_Maui.Views;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // coordinates of the school
        var latitude = 54.9130415;
        var longitude = 9.77896211;

        var location = new Location(latitude, longitude);

        // sets the map region to show the area around the coordinates (8 km radius)
        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(8));
        TheMap.MoveToRegion(mapSpan);

        // create and add a pin to the map at the desired location
        var pin = new Pin
        {
            Label = "Device #",
            Location = location,
            Type = PinType.Place
        };
        TheMap.Pins.Add(pin);

        
    }

}
