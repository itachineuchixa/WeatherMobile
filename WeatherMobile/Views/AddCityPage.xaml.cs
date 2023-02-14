using WeatherMobile.ViewModels;
namespace WeatherMobile;

public partial class AddCityPage : ContentPage
{
	public AddCityPage()
	{
		InitializeComponent();
		this.BindingContext = new AddCity() { Navigation = this.Navigation };
	}
}