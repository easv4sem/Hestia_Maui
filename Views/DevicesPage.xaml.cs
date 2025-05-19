namespace Hestia_Maui.Views;

public partial class DevicesPage : ContentPage
{
	public DevicesPage(DevicesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
