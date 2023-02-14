namespace WeatherMobile;

public partial class RegPage : ContentPage
{
	public RegPage()
	{
		InitializeComponent();
        this.BindingContext = new Reg() { Navigation = this.Navigation };
    }
}