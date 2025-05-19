using CommunityToolkit.Mvvm.Messaging.Messages;


namespace Hestia_Maui.MessageTypes
{
    // defines an info message type used for publishing info notifications via the messenger
    public class InfoMessage : ValueChangedMessage<string>
    {
        public InfoMessage(string message) : base(message) { }
    }
}
