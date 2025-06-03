using Hestia_Maui.Service;
using Hestia_Maui.Models;
using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;
using System.Net;
using Hestia_Maui.Interface;

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



    private readonly IApiService _apiServices;

    public AddDeviceViewModel(IApiService apiservice)
    {
        _apiServices = apiservice;
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
        if (string.IsNullOrEmpty(IdText) || string.IsNullOrEmpty(NameText) || LatitudeMaps == 0 || LongitudeMaps == 0) {
            
            Debug.WriteLine("Some credentials are missing creating device");
            WeakReferenceMessenger.Default.Send(new InfoMessage("Please check your credentials - some are missing"));
            return;
        }


        var cookie = await SecureStorage.GetAsync("session_cookie");

        if (!string.IsNullOrEmpty(cookie))
        {
            bool isUpdated = await PutData();

            if (!isUpdated)
            {
                WeakReferenceMessenger.Default.Send(new InfoMessage("Could not add the device. Please try again later"));
                Debug.WriteLine("Device update failed.");
                return;
            }

            // Reset input fields og naviger til HomePage hvis opdatering lykkedes
            Reset();
            WeakReferenceMessenger.Default.Send(new InfoMessage("Device added successfully"));
            await Shell.Current.GoToAsync("///HomePage");
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new InfoMessage("Your session has expired. Please log in again to continue."));
            Debug.WriteLine("Session cookie is missing or expired.");
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
        const string endpoint = "api/devices";

        var deviceData = new DeviceData
        {
            Id = IdText,
            DisplayName = NameText,
            Latitude = LatitudeMaps,
            Longitude = LongitudeMaps
        };

        // checks data
        if (!string.IsNullOrWhiteSpace(deviceData.Id) && deviceData.Latitude != 0 && deviceData.Longitude != 0)
        {
            try
            {
                var requestWrapper = new PutDeviceRequest 
                {
                    Device = deviceData
                };


                var response = await _apiServices.ApiPutAsync<PutDeviceRequest>(endpoint, requestWrapper);

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
        NameText = string.Empty;
    }

}
