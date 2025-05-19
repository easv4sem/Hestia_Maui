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
    public string entryHeador = " Raspberry Pi ID:";
    
    [ObservableProperty]
    public string idPlaceholder = "AA:BB:CC:00:00:11";
    
    [ObservableProperty]
    public string btnCancelText = "Cancel";
    
    [ObservableProperty]
    public string btnOkText = "OK";


    // two-way bound property: UI and ViewModel update each other
    [ObservableProperty]
    public string idText = "";



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
        // resets the device input fields and navigates back to the home page
        Reset();
        await Shell.Current.GoToAsync("///HomePage");
    }


    /// <summary>
    /// Command handler for the "OK" button.
    /// Checks for a valid session cookie and posts the device data.
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
    /// Returns true if the post was successful, otherwise false.
    /// </summary>
    /// <returns>True if device data was posted successfully; false otherwise.</returns>
    private async Task<bool> PutData()
    {
        string endpoint = "";

        DeviceData deviceData = new DeviceData
        {
            Id = IdText,
            DisplayName = "NOGET HER!",
            Latitude = 54.9130415,
            Longitude = 9.77896211
        };


        if (deviceData.Id != "" && deviceData.Longitude != 0 && deviceData.Latitude != 0)
        {
            try
            {
                return await _apiServices.ApiPostData(endpoint, deviceData);
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Error PutData: {ex.Message}");
                return false;
            }
        }
        else
        {
            Debug.WriteLine("Some device data are missing...");
            return false;
        }

    }


    /// <summary>
    /// Resets the device input field
    /// </summary>
    private void Reset()
    {
        IdText = string.Empty;
    }

}
