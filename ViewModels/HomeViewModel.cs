namespace Hestia_Maui.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    // one-way bound properties: ViewModel updates UI only
    [ObservableProperty]
    public string labelHeader = "Welcome";

    [ObservableProperty]
    public string labelLine = "to Hestia";

    [ObservableProperty]
    public string btnSeeDevicesText = "See devices";

    [ObservableProperty]
    public string btnAddDevicesText = "Add device";



    [RelayCommand]
    private async Task SeeDevicePressed()
    {
        await Shell.Current.GoToAsync("///DevicesPage");
    }



    [RelayCommand]
    private async Task AddDevicePressed()
    {
        await Shell.Current.GoToAsync("///AddDevicePage");
    }
}

