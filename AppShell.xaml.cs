using System.Net;

namespace Hestia_Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Handles the logout action when the "Logout" button is clicked.
    /// Displays a confirmation dialog to the user, and if confirmed,
    /// clears the session cookie and navigates back to the login page.
    /// </summary>
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Log out",
            "Are you sure you want to log out?",
            "Yes",
            "No");

        if (confirm)
        {
            // removes the stored session cookie from secure storage
            SecureStorage.Remove("session_cookie");

            // navigates to login page 
            await Shell.Current.GoToAsync("///UrlOrganisationPage");
        }
    }
}
