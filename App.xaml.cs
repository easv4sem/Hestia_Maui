using Hestia_Maui.Service;

namespace Hestia_Maui;

public partial class App : Application
{

    private readonly NotificationService _notificationService;

    public App()
	{
		InitializeComponent();

        _notificationService = new NotificationService();
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}
