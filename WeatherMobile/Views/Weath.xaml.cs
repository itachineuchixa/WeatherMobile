namespace WeatherMobile;

public partial class Weath : ContentPage
{
	public Weath()
	{
		InitializeComponent();
		this.BindingContext = new ViewModel();
	}
}