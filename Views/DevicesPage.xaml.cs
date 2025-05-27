namespace Hestia_Maui.Views;

public partial class DevicesPage : ContentPage
{
	public DevicesPage(DevicesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        this.Appearing += DevicesPage_Appearing;
    }

    // Event handler for when the page appears on screen
    private void DevicesPage_Appearing(object sender, EventArgs e)
    {
        if (BindingContext is DevicesViewModel vm)
        {
            if (vm.LoadDevicesCommand.CanExecute(null))
            {
                // Execute the command to load devices or perform related logic
                vm.LoadDevicesCommand.Execute(null);
            }
        }
    }
}
