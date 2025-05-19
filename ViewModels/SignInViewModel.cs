using Hestia_Maui.Service;
using Hestia_Maui.Models;
using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;

namespace Hestia_Maui.ViewModels;

public partial class SignInViewModel : BaseViewModel
{
    // one-way bound properties: ViewModel updates UI only
    [ObservableProperty]
    public string headerText = " Login to your";

    [ObservableProperty]
    public string lineText = "Organization";

    [ObservableProperty]
    public string btnBackText = "Back";

    [ObservableProperty]
    public string btnLoginText = "Login";

    [ObservableProperty]
    public string emailPlaceholder = "example@google.com";
    
    [ObservableProperty]
    public string passwordPlaceholder = "**********";


    // two-way bound property: UI and ViewModel update each other
    [ObservableProperty]
    public string email = "";
    
    [ObservableProperty]
    public string password = "";



    // URL endpoint for posting login data to the API
    private readonly string _apiPutLoginEndpoint = "https://8c7567d1-8477-484d-ab9c-e88dcf0aa29e.mock.pstmn.io/api/maui_post";
    private readonly ApiServices _apiServices;

    public SignInViewModel()
    {
        _apiServices = new ApiServices();
    }


    // TODO: REMOVE AGAIN!
    public string cookieString = "";


    /// <summary>
    /// Command handler for the "Back" button.
    /// Resets relevant input fields and navigates to the organization URL page.
    /// </summary>
    [RelayCommand]
    private async Task BackPressed()
    {
        // resets device properties connected to pi and navigates to home page
        Reset();
        await Shell.Current.GoToAsync("///UrlOrganisationPage");

    }


    /// <summary>
    /// Command handler for the "Login" button.
    /// Validates that both email and password are entered.
    /// If valid, attempts login, stores session cookie, resets input fields
    /// and navigates to the HomePage. 
    /// </summary>
    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrEmpty(Email))
        {
            // publish an ErrorMessage which NotificationService is subscribed to
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Please enter an email"));
        } 
        
        else if (string.IsNullOrEmpty(Password))
        {
            // publish an ErrorMessage which NotificationService is subscribed to
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Please enter password"));
        }
        
        else
        {
            bool isLoggedIn = await LoginToApp();
            // saves session cookie in encrypted device storage
            await SecureStorage.SetAsync("session_cookie", cookieString);

            // resets entered email and password and navigates to home page
            Reset();
            await Shell.Current.GoToAsync("///HomePage");
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<bool> LoginToApp()
    {
        LoginData loginData = new LoginData
        {

        };

        return true;
    }


    /// <summary>
    /// Resets the login input fields
    /// </summary>
    private void Reset()
    {
        Email = string.Empty;
        Password = string.Empty;
    }
}
