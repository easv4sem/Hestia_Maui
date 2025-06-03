using Hestia_Maui.Service;
using Hestia_Maui.Models;
using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;
using Hestia_Maui.Interface;
using System.Text.RegularExpressions;

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
    private readonly IApiService _apiServices;

    public SignInViewModel(IApiService apiService)
    {
        _apiServices = apiService;
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


        try
        {

            bool isLoggedIn = await LoginToApp();

            if (isLoggedIn)
            {
                // saves session cookie in encrypted device storage
                //await SecureStorage.SetAsync("session_cookie", cookieString);

                // resets entered email and password and navigates to home page
                Reset();
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                Debug.WriteLine("Login failed");
                WeakReferenceMessenger.Default.Send(new ErrorMessage("Login failed. Please check your credentials."));
            }
        }
        catch (Exception ex) { 
        
            Debug.WriteLine($"Error during login: {ex}");
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Couldn't login. Please try again later"));
        }
    }





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
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    string fullCookie = cookies.FirstOrDefault();
                    Debug.WriteLine($"Raw Set-Cookie header: {fullCookie}");

                    // Parse cookie value using Regex
                    // Example header: user=abcdef12345; Path=/; HttpOnly; SameSite=Lax
                    string extractedValue = ExtractCookieValue(fullCookie, "user");
                    if (!string.IsNullOrEmpty(extractedValue))
                    {
                        await SecureStorage.SetAsync("session_cookie", extractedValue);
                        Debug.WriteLine($"Stored cookie value: {extractedValue}");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("No valid 'user' cookie found in header.");
                        return false;
                    }
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
    /// Extracts the cookie value for the given cookie name from a Set-Cookie header string.
    /// </summary>
    private string ExtractCookieValue(string setCookieHeader, string cookieName)
    {
        if (string.IsNullOrEmpty(setCookieHeader) || string.IsNullOrEmpty(cookieName))
            return string.Empty;

        // Use Regex to extract the cookie value
        var match = Regex.Match(setCookieHeader, $@"{cookieName}=([^;]+)");
        return match.Success ? match.Groups[1].Value : string.Empty;
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
