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

    public event EventHandler<bool> GoingBackToMain;

    protected override bool OnBackButtonPressed() {
        GoingBackToMain?.Invoke(this, true);
        return base.OnBackButtonPressed();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e) {
        GoingBackToMain?.Invoke(this, true);
        set.SaveJson();
        await Navigation.PopAsync();
    }
}