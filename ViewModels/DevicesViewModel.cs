using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;
using Hestia_Maui.Models;
using Hestia_Maui.Models.DTO;
using Hestia_Maui.Service;
using Hestia_Maui.Interface;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Dispatching;
using System.Net;


namespace Hestia_Maui.ViewModels
{
    public partial class DevicesViewModel : BaseViewModel
    {
        // one-way bound properties: ViewModel updates UI only
        [ObservableProperty]
        public string greenDot = "icon_green_circle.png";

        [ObservableProperty]
        public string redDot = "icon_red_circle.png";

        [ObservableProperty]
        public string labelActiveDevice = "Active device";

        [ObservableProperty]
        public string labelInactiveDevice = "Not active device";


        public ObservableCollection<DeviceDisplayProps> ActiveDevices { get; } = new();
        private readonly IApiService _apiService;

        public DevicesViewModel(IApiService apiservice)
        {
            _apiService = apiservice;
        }


        [RelayCommand]
        private async Task LoadDevicesAsync()
        {
            try
            {
                var devices = await _apiService.ApiGetAll<DeviceDTO>("api/devices/");

                if (devices == null || !devices.Any())
                {
                    Debug.WriteLine("Received no devices or failed to load devices");
                    WeakReferenceMessenger.Default.Send(new InfoMessage("Couldn't load any devices. Please try again"));
                    await Shell.Current.GoToAsync("///HomePage");
                    return;
                }

                var sortedDevices = DeviceUtility.ConvertAndSortActiveDevices(devices);

                ActiveDevices.Clear();
                foreach (var device in sortedDevices)
                {
                    ActiveDevices.Add(device);
                    Debug.WriteLine(device);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading devices: {ex.Message}");
            }
        }
    }
}
