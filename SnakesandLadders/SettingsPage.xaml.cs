namespace SnakesandLadders;

public partial class SettingsPage : ContentPage
{
	Settings set;
	public SettingsPage(Settings s)
	{
		this.set = s;
		InitializeComponent();
		BindingContext = set;
	}

    private async void SaveButton_Clicked(object sender, EventArgs e) {
        set.SaveJson();
        await Navigation.PopAsync();
    }
}