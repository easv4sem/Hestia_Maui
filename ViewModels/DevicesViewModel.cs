using Hestia_Maui.Models;
using Hestia_Maui.Models.DTO;
using Hestia_Maui.Service;


namespace Hestia_Maui.ViewModels
{
    public partial class DevicesViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceDisplayProps> ActiveDevices { get; } = new();

        private ApiServices _apiService;

        public DevicesViewModel()
        {
            _apiService = new ApiServices();
        }






        private DeviceDisplayProps _selectedDevice;
        public DeviceDisplayProps SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetProperty(ref _selectedDevice, value))
                {
                    UpdateTooltipText();
                }
            }
        }

        private string _tooltipText;
        public string TooltipText
        {
            get => _tooltipText;
            set
            {
                if (SetProperty(ref _tooltipText, value))
                {
                    OnPropertyChanged(nameof(TooltipBackgroundColor));
                    OnPropertyChanged(nameof(IsTooltipVisible));
                }
            }
        }

        // Background color changes based on whether tooltip text exists
        public Color TooltipBackgroundColor => string.IsNullOrEmpty(TooltipText) ? Colors.Transparent : Colors.AliceBlue;

        // Label visibility is tied to whether tooltip text is empty or not
        public bool IsTooltipVisible => !string.IsNullOrEmpty(TooltipText);

        private CancellationTokenSource _tooltipCts;

        private void UpdateTooltipText()
        {
            if (SelectedDevice == null)
            {
                TooltipText = string.Empty;
                return;
            }

            TooltipText = SelectedDevice.HasHeartbeat ? "Device is active" : "Device is not active";

            // Cancel previous hide-timer if any
            _tooltipCts?.Cancel();
            _tooltipCts = new CancellationTokenSource();
            var ct = _tooltipCts.Token;

            // Hide tooltip after 5 seconds
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(5000, ct);
                    if (!ct.IsCancellationRequested)
                    {
                        TooltipText = string.Empty;
                    }
                }
                catch (TaskCanceledException)
                {
                    // ignore cancellation
                }
            });
        }

        [RelayCommand]
        private async Task LoadDevicesAsync()
        {
            try
            {
                Debug.WriteLine(">>> LoadDevicesAsync CALLED");
                var devices = await _apiService.ApiGetAll<DeviceDTO>("api/devices");
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
