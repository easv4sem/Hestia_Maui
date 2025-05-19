using CommunityToolkit.Mvvm.Messaging.Messages;


namespace Hestia_Maui.MessageTypes
{
    // defines an error message type used for publishing error notifications via the messenger
    public class ErrorMessage : ValueChangedMessage<string>
    {
        public ErrorMessage(string message) : base(message) { }
    }
}
