namespace Hestia_Maui.Views;

public partial class UrlOrganisationPage : ContentPage
{
	public UrlOrganisationPage(UrlOrganisationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
