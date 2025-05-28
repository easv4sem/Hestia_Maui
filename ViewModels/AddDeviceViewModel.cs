using Hestia_Maui.Service;
using Hestia_Maui.Models;
using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;

namespace Hestia_Maui.ViewModels;

public partial class AddDeviceViewModel : BaseViewModel
{
    // one-way bound properties: ViewModel updates UI only
    [ObservableProperty]
    public string pageText = "Add new device:";
    
    [ObservableProperty]
    public string entryIdHeador = " Raspberry Pi ID:";
    
    [ObservableProperty]
    public string idPlaceholder = "AA:BB:CC:00:00:11";

    [ObservableProperty]
    public string entryNameHeador = " Device name:";

    [ObservableProperty]
    public string namePlaceholder = "Lottenhoi";

    [ObservableProperty]
    public string btnCancelText = "Cancel";
    
    [ObservableProperty]
    public string btnOkText = "OK";


    // two-way bound property: UI and ViewModel update each other
    [ObservableProperty]
    public string idText = "";

    [ObservableProperty]
    public string nameText = "";


    // Coordinates for map location
    [ObservableProperty]
    private float latitudeMaps;

    [ObservableProperty]
    private float longitudeMaps;



    private readonly ApiServices _apiServices;

    public AddDeviceViewModel()
    {
        _apiServices = new ApiServices();
    }



    /// <summary>
    /// Command handler for the "Cancel" button.
    /// Resets the input fields and navigates back to the HomePage.
    /// </summary>
    [RelayCommand]
    private async Task CancelPressed()
    {
        Debug.WriteLine(LatitudeMaps);
        Debug.WriteLine(LongitudeMaps);

        // resets the device input fields and navigates back to the home page
        Reset();
        await Shell.Current.GoToAsync("///HomePage");
    }


    /// <summary>
    /// Command handler for the "OK" button.
    /// Checks for a valid session cookie and puts the device data.
    /// If the session cookie is missing or expired, navigates to SignInPage.
    /// If success - resets input and navigates to HomePage.
    /// </summary>
    [RelayCommand]
    private async Task OkPressed()
    {
        var cookie = await SecureStorage.GetAsync("session_cookie");

        if (!string.IsNullOrEmpty(cookie))
        {
            bool isUpdatet = await PutData();

            if (!isUpdatet)
            {
                WeakReferenceMessenger.Default.Send(new InfoMessage("The device wasn't updatet."));
                Debug.WriteLine("Wasn't updated");
                return;
            }

            // resets the device input fields and navigates back to the home page
            Reset();
            await Shell.Current.GoToAsync("///HomePage");
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new InfoMessage("Your session has expired. Please log in again to continue."));
            Debug.WriteLine("Cookie is expired");
            await Shell.Current.GoToAsync("///SignInPage");
        }

    }


    /// <summary>
    /// Sends the current device data to the API.
    /// Validates that all required data is present before sending.
    /// Returns true if the PUT request was successful; false otherwise.
    /// </summary>
    private async Task<bool> PutData()
    {
        //TODO: change!
        const string endpoint = "api/device/"; 

        var deviceData = new DeviceData
        {
            Id = IdText,
            DisplayName = NameText,
            Latitude = LatitudeMaps,
            Longitude = LongitudeMaps
        };

        if (!string.IsNullOrWhiteSpace(deviceData.Id) && deviceData.Latitude != 0 && deviceData.Longitude != 0)
        {
            try
            {
                var response = await _apiServices.ApiPutAsync<DeviceData>(endpoint, deviceData);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Device updated successfully.");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Update failed. StatusCode: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error [PutData]: {ex.Message}");
                return false;
            }
        }

        Debug.WriteLine("Missing or invalid device data.");
        return false;
    }


    /// <summary>
    /// Resets the device input field
    /// </summary>
    private void Reset()
    {
        IdText = string.Empty;
    }

}
