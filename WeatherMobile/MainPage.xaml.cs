namespace WeatherMobile;
using WeatherMobile.ViewModels;
public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = new Login() { Navigation = this.Navigation };
	}

}

