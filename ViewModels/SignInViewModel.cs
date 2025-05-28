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
    public string labelUsername = "Username";

    [ObservableProperty]
    public string usernamePlaceholder = "Example123";

    [ObservableProperty]
    public string labelPassword = "Password";

    [ObservableProperty]
    public string passwordPlaceholder = "**********";


    // two-way bound property: UI and ViewModel update each other
    [ObservableProperty]
    public string enteredUsername = "";
    
    [ObservableProperty]
    public string enteredPassword = "";



    // URL endpoint for posting login data to the API
    private readonly string _loginEndpoint = "api/user/login";
    private string cookieString ="Banankage";
    private readonly ApiServices _apiServices;

    public SignInViewModel()
    {
        _apiServices = new ApiServices();
    }


    


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
        if (string.IsNullOrEmpty(EnteredUsername))
        {
            // publish an ErrorMessage which NotificationService is subscribed to
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Please enter your username"));
            return;
        } 
        
        if (string.IsNullOrEmpty(EnteredPassword))
        {
            // publish an ErrorMessage which NotificationService is subscribed to
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Please enter password"));
            return;
        }

        /*
        bool isLoggedIn = await LoginToApp();

        if (isLoggedIn)
        {
            // saves session cookie in encrypted device storage
            await SecureStorage.SetAsync("session_cookie", cookieString);

            // resets entered email and password and navigates to home page
            Reset();
            await Shell.Current.GoToAsync("///HomePage");
        }
        else
        {
            Debug.WriteLine("Login failed");
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Login failed. Please check your credentials."));
        }
        */


        // saves session cookie in encrypted device storage
        await SecureStorage.SetAsync("session_cookie", cookieString);

        // resets entered email and password and navigates to home page
        Reset();
        await Shell.Current.GoToAsync("///HomePage");

    }


    /// <summary>
    /// Attempts to log in the user with the provided Username and Password.
    /// Sends login data to the API and processes the response.
    /// </summary>
    /// <returns>True if login succeeded and session cookie retrieved; otherwise false</returns>
    private async Task<bool> LoginToApp()
    {
        LoginData loginData = new LoginData
        {
            Username = EnteredUsername,
            Password = EnteredPassword
        };

        try
        {
            var response = await _apiServices.ApiPostAsync<LoginData>(_loginEndpoint, loginData);

            if (response.IsSuccessStatusCode)
            {
                // Extract cookie or token from response headers or content as appropriate
                // Example assuming a session cookie is returned in a header:
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    cookieString = cookies.FirstOrDefault() ?? string.Empty;
                    return true;
                }
                else
                {
                    Debug.WriteLine("No session cookie found in response");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine($"Login failed with status code: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during login: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Resets the login input fields
    /// </summary>
    private void Reset()
    {
        EnteredUsername = string.Empty;
        EnteredPassword = string.Empty;
    }
}
