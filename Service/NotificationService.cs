using CommunityToolkit.Mvvm.Messaging;
using Hestia_Maui.MessageTypes;


namespace Hestia_Maui.Service
{
    /// <summary>
    /// Service that listens for InfoMessage and ErrorMessage notifications
    /// and displays corresponding alerts in the UI
    /// </summary>
    public class NotificationService : IRecipient<InfoMessage>, IRecipient<ErrorMessage>
    {
        public NotificationService()
        {
            // subscribes to receive InfoMessage and ErrorMessage notifications
            WeakReferenceMessenger.Default.Register<InfoMessage>(this);
            WeakReferenceMessenger.Default.Register<ErrorMessage>(this);
        }


        /// <summary>
        /// Handles incoming InfoMessage by showing an info alert
        /// </summary>
        public void Receive(InfoMessage message)
        {
            ShowAlert("Info", message.Value);
        }


        /// <summary>
        /// Handles incoming ErrorMessage by showing an error alert
        /// </summary>
        public void Receive(ErrorMessage message)
        {
            ShowAlert("Error", message.Value);
        }


        /// <summary>
        /// Displays the alert dialog on the main UI thread
        /// </summary>
        private async void ShowAlert(string title, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.DisplayAlert(title, message, "OK");
            });
        }
    }
}
