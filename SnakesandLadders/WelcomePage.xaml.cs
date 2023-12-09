namespace SnakesandLadders;

public partial class WelcomePage : ContentPage
{
    private int numberplayers;

	public WelcomePage()
	{
		InitializeComponent();
        numberplayers = Preferences.Default.Get("numberplayers",2);
        noPlayersText.Text = stepPlayers.Value.ToString() + " players selected, enter their names";
        HideSomeRows();
        BindingContext = this;
    }

    private void stepPlayers_ValueChanged(object sender, ValueChangedEventArgs e) {
		noPlayersText.Text = stepPlayers.Value.ToString() +" players selected, enter their names";
        numberplayers = (int)stepPlayers.Value;
        HideSomeRows();
    }

    private void HideSomeRows() {
        if (numberplayers == 4)
            PlayerNameGrid.RowDefinitions[3].Height = 50;
        else
            PlayerNameGrid.RowDefinitions[3].Height = 0;
        if (numberplayers >= 3)
            PlayerNameGrid.RowDefinitions[2].Height = 50;
        else
            PlayerNameGrid.RowDefinitions[2].Height = 0;
        PlayerNameGrid.HeightRequest = 50 * numberplayers;
    }

    private async void PlayGame_Clicked(object sender, EventArgs e) {
        Preferences.Default.Set("numberplayers", numberplayers);
        for (int i = 0; i < numberplayers; i++) {
            string thisname = "";
            switch (i) {
                case 0: thisname = player1Entry.Text; break;
                case 1: thisname = player2Entry.Text; break;
                case 2: thisname = player3Entry.Text; break;
                case 3: thisname = player4Entry.Text; break;
            }
            if (thisname == null || thisname.Length == 0) {
                await DisplayAlert("Enter Player Name", "You need to enter player " + (i + 1) + "'s name", "OK");
                return;
            }
            Preferences.Default.Set("Player" + i, thisname);
        }
        await Shell.Current.GoToAsync("//MainPage", true);
    }

}