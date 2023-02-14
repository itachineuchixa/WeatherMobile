namespace WeatherMobile;

public partial class DataPage : ContentPage
{
	public DataPage()
	{
		InitializeComponent();
		this.BindingContext = new ViewModelDataPage() { Navigation = this.Navigation };
	}
}