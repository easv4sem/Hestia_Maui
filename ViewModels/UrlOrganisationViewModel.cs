using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;

namespace Hestia_Maui.ViewModels;

public partial class UrlOrganisationViewModel : BaseViewModel
{
    // one-way bound properties: ViewModel updates UI only
    [ObservableProperty]
    public string headerText = " Direct us to your";

    [ObservableProperty]
    public string lineText = "Organization";

    [ObservableProperty]
    public string labelText = "Organization url:";

    [ObservableProperty]
    public string btnHelpText = "Help";

    [ObservableProperty]
    public string btnNextText = "Next";

    [ObservableProperty]
    public string urlPlaceholder = "https://yourOrg[dot]com";


    // two-way bound property: UI and ViewModel update each other
    [ObservableProperty]
    public string enteredOrganisationUrl = "";



    /// <summary>
    /// Command handler for the "Help" button.
    /// When pressed, notifies the user with information about entering the URL.
    /// </summary>
    [RelayCommand]
    private async Task HelpPressed()
    {
        // publish an InfoMessage which NotificationService is subscribed to
        WeakReferenceMessenger.Default.Send(new InfoMessage("Please provide the complete organization URL as given by your administrator to proceed"));
    }


    /// <summary>
    /// Command handler for the "Next" button.
    /// Checks if OrganisationUrl is missing and shows an error if so.
    /// Otherwise, saves the URL in secure storage and navigates to the SignIn page.
    /// </summary>
    [RelayCommand]
    private async Task NextPressed()
    {
        if (string.IsNullOrEmpty(EnteredOrganisationUrl))
        {
            // publish an ErrorMessage which NotificationService is subscribed to
            WeakReferenceMessenger.Default.Send(new ErrorMessage("Please enter an organization URL"));
        }
        else
        {
            // saves organisation url in encrypted device storage
            await SecureStorage.SetAsync("organisation_url", EnteredOrganisationUrl);

            // resets entered organisation url and navigates to sign in page
            Reset();
            await Shell.Current.GoToAsync("///SignInPage");
        }
    }



    /// <summary>
    /// Resets the organization input field
    /// </summary>
    private void Reset()
    {
        EnteredOrganisationUrl = string.Empty;
    }
}
