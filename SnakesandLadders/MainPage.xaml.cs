using System.Text.Json;
using Microsoft.Maui.Controls.Shapes;
using Plugin.Maui.Audio;

namespace SnakesandLadders
{
    public partial class MainPage : ContentPage
    {
        Random random;
        private bool rollingdice = false;
        private int movingplayer = -1;
        private bool movingsnladders = false;
        private int winnerOfGame = -1;
        private List<SnakeLadder> snakesladdersList;
        const int DICE_DELAY = 250;
        private List<Player> players;
        private int playerTurn;
        private int numberOfPlayers;
        private Settings set;
        private IAudioPlayer audioplayer;
        private int countToChangeSnakes;
        private bool everythingInitialised = false;
        private bool fromsettingspage = false;
        private bool loadingpage = false;
        private bool snakeshavemoved;

        public bool LoadingPage
        {
            get => loadingpage;
            set
            {
                loadingpage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EverythingLoaded));
            }
        }

        public bool EverythingLoaded => !LoadingPage;


        public bool ShowTwoDice
        {
            get
            {
                if (set != null)
                    return set.TwoDice;
                else
                    return false;
            }
            set
            {
                set.TwoDice = value;
                OnPropertyChanged();
            }
        }

        private int WinnerGame
        {
            get => winnerOfGame;
            set
            {
                if (winnerOfGame != value) {
                    winnerOfGame = value;
                    OnPropertyChanged(nameof(TopText));
                }
            }
        }

        private bool RollingDice
        {
            get => rollingdice;
            set
            {
                if (rollingdice != value) {
                    rollingdice = value;
                    OnPropertyChanged(nameof(TopText));
                }
            }
        }

        private int MovingPlayer
        {
            get => movingplayer;
            set
            {
                if (movingplayer != value) {
                    movingplayer = value;
                    OnPropertyChanged(nameof(TopText));
                }
            }
        }

        private bool MovingSnakesLadders
        {
            get => movingsnladders;
            set
            {
                if (movingsnladders != value) {
                    movingsnladders = value;
                    OnPropertyChanged(nameof(TopText));
                }
            }
        }

        public string TopText
        {
            get
            {
                if (players == null || players.Count == 0)
                    return "";
                if (WinnerGame != -1)
                    return "Well done " + players[playerTurn].name + " you have won!!\nIf you'd like to start again, click the New Game Button";
                else if (MovingSnakesLadders)
                    return "Please Wait, Moving Snakes and Ladders!!";
                else if (MovingPlayer != -1)
                    return "Moving " + players[playerTurn].name + " by " + MovingPlayer + " spaces";
                else if (RollingDice)
                    return "Rolling Dice, let's see what you get";
                else
                    return players[playerTurn].name + ", it's your go - Roll the Dice";
            }
        }
        public MainPage() {
            InitializeComponent();
           // this.LayoutChanged += OnWindowChange;
           // this.initTask =
            
        }

        private async Task InitialiseObjectVariables() {
            string settingsfilename = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "settings.json");
            if (File.Exists(settingsfilename)) {
                try {
                    using (StreamReader reader = new StreamReader(settingsfilename)) {
                        string jsonstring = reader.ReadToEnd();
                        set = JsonSerializer.Deserialize<Settings>(jsonstring);
                    }
                }
                catch {
                    set = new Settings();
                }
            }
            else
                set = new Settings();
            UpdateSettings();
            SetUpGrid();
            InitRandomDice();
            
            MakeSnakesLadders();
            audioplayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("dicerolling.mp3"));
            everythingInitialised = true;
        }

        private void UpdateSettings() {
            countToChangeSnakes = (set.SnakesLaddersChange == 0) ? -1 : set.SnakesLaddersChange;
            ShowTwoDice = set.TwoDice;
            Resources["GridColour1"] = Color.FromArgb(set.GRID_COLOUR1);
            Resources["GridColour2"] = Color.FromArgb(set.GRID_COLOUR2);
            Resources["DiceFgColour"] = Color.FromArgb(set.DICE_COLOURFG);
            Resources["DiceBgColour"] = Color.FromArgb(set.DICE_COLOURBG);
            if (set.MoveSnakes != SnakeLadder.snakesmove) {
                SnakeLadder.snakesmove = set.MoveSnakes;
                if (snakesladdersList != null) {
                    foreach (var snake in snakesladdersList)
                        snake.ChangeMovement();
                }
            }
        }

    /*    private void OnWindowChange(object sender, EventArgs e) {
            if (this.Width <= 0)
                return;
            if (this.Width < 480) {
                int newdim = (int)this.Width / 10;
                double rescale = (double)newdim * 10 / 480;
                GridGameTable.Scale = rescale;
                TopTextLbl.WidthRequest = newdim * 10;
                //windowScale = rescale;
            }
        }*/

        private void MakeSnakesLadders() {
            SnakeLadder.grid = GridGameTable;
            SnakeLadder.snakesmove = set.MoveSnakes;
            if(snakesladdersList == null)
                snakesladdersList = new();
            else {
                foreach(var piece in snakesladdersList) {
                    piece.RemoveImage();
                }
                snakesladdersList.Clear();
            }

            snakesladdersList.Add(new SnakeLadder(6, 5, 4, 4));
            snakesladdersList.Add(new SnakeLadder(7, 5, 2, 2));
            snakesladdersList.Add(new SnakeLadder(3, 2, 3, 4));
            snakesladdersList.Add(new SnakeLadder(4, 0, 9, 9));
            snakesladdersList.Add(new SnakeLadder(5, 4, 9, 7));
            snakesladdersList.Add(new SnakeLadder(2, 4, 8, 6));

            //Snakes
            snakesladdersList.Add(new SnakeLadder(0, 3, 4, 6));
            snakesladdersList.Add(new SnakeLadder(3, 6, 9, 7));

            snakesladdersList.Add(new SnakeLadder(2, 7, 6, 6));
            snakesladdersList.Add(new SnakeLadder(1, 4, 3, 3));
            snakesladdersList.Add(new SnakeLadder(5, 7, 4, 3));
            snakesladdersList.Add(new SnakeLadder(7, 9, 1, 2));

            snakesladdersList.Add(new SnakeLadder(7, 8, 6, 5));
            snakesladdersList.Add(new SnakeLadder(0, 1, 1, 2));

            snakesladdersList.Add(new SnakeLadder(9, 30));
            snakesladdersList.Add(new SnakeLadder(25, 14));
            snakesladdersList.Add(new SnakeLadder(80, 95));
            snakeshavemoved = false;


        }
        private void InitialisePlayers(int howmany) {
            Player.grid = GridGameTable;
            Player.mustRoll100 = set.Finish100;
            players = new List<Player>();
            playerTurn = 0;
            string[] defaults = { "John", "Mary", "Luke", "Leia" };
            for (int i = 0; i < howmany; i++) {
                players.Add(new Player(Preferences.Default.Get("Player" + i, defaults[i]), i + 1));
            }
            numberOfPlayers = howmany;
        }

        private void InitRandomDice() {
            random = new();
            FillDiceGrid(random.Next(1, 7), DiceGrid);
            FillDiceGrid(random.Next(1, 7), DiceGrid2);
        }

        private Ellipse drawcircle() {
            Ellipse ell = new Ellipse()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            ell.SetDynamicResource(Ellipse.FillProperty, "DiceFgColour");
            return ell;
        }

        private void ClearDiceGrid(Grid grid) {
            if (grid == null) return;
            List<View> childrenToRemove = new();
            foreach (var item in grid.Children) {
                if (item.GetType() == typeof(Ellipse)) {
                    childrenToRemove.Add((Ellipse)item);
                }
            }
            //Actually remove them from the Grid
            foreach (var item in childrenToRemove) {
                grid.Remove(item);
            }
        }

        private void FillDiceGrid(int i, Grid grid) {
            switch (i) {
                case 1:
                    grid.Add(drawcircle(), 1, 1);
                    break;
                case 2:
                    grid.Add(drawcircle(), 0, 0);
                    grid.Add(drawcircle(), 2, 2);
                    break;
                case 3:
                    for (int j = 0; j < 3; j++) {
                        grid.Add(drawcircle(), j, j);
                    }
                    break;
                case 4:
                    for (int j = 0; j < 3; j += 2) {
                        for (int k = 0; k < 3; k += 2) {
                            grid.Add(drawcircle(), j, k);
                        }
                    }
                    break;
                case 5:
                    for (int j = 0; j < 3; j += 2) {
                        for (int k = 0; k < 3; k += 2) {
                            grid.Add(drawcircle(), j, k);
                        }
                    }
                    grid.Add(drawcircle(), 1, 1);
                    break;
                case 6:
                    for (int j = 0; j < 3; j += 2) {
                        for (int k = 0; k < 3; ++k) {
                            grid.Add(drawcircle(), k, j);
                        }
                    }
                    break;
            }
        }

        private async Task AnimateRollingDice(int howmany, Border diceb, Grid diceg, int[] pattern) {

            int DiceBorderCurrentCol = Int32.Parse(diceb.GetValue(Grid.ColumnProperty).ToString());
            int DiceBorderSetCol = 0;
            double xStep = GridGameTable.Width / 10.0;

            //This translation works well on Windows but it is not quite right on Android
            //Has a bit of stutter at the end. * 4.0 is better on Android but still a little off
            double translation = (DeviceInfo.Current.Platform == DevicePlatform.Android) ? 4.0 : 4.5;
            if (DiceBorderCurrentCol == 2 || DiceBorderCurrentCol == 4) {
                diceb.TranslateTo(xStep * translation, 0, (uint)howmany * DICE_DELAY);
                DiceBorderSetCol = 6 + DiceBorderCurrentCol - 2;
            }
            else {
                diceb.TranslateTo(-xStep * translation, 0, (uint)howmany * DICE_DELAY);
                DiceBorderSetCol = 2 + DiceBorderCurrentCol - 6;
            }
            diceb.RotationY = 0;
            for (int i = 0; i < howmany; ++i) {
                await diceb.RotateYTo(diceb.RotationY + 90, DICE_DELAY / 2);
                ClearDiceGrid(diceg);
                FillDiceGrid(pattern[i], diceg);
                await diceb.RotateYTo(diceb.RotationY + 90, DICE_DELAY / 2);
            }
            diceb.TranslationX = 0;
            diceb.SetValue(Grid.ColumnProperty, DiceBorderSetCol);
        }

        private async Task RollDiceUsingGrid() {
            int howmany = random.Next(4, 10);

            int count = 0;
            int which = 0;
            int last = 0;
            int[] pattern = new int[howmany];
            for (int i = 0; i < howmany; i++) {
                do {
                    which = random.Next(1, 7);
                } while (which == last);
                last = which;
                pattern[i] = last;
            }
            count += which;
            if (set.TwoDice) {
                int[] pattern2 = new int[howmany];
                for (int i = 0; i < howmany; i++) {
                    do {
                        which = random.Next(1, 7);
                    } while (which == last);
                    last = which;
                    pattern2[i] = last;
                }
                AnimateRollingDice(howmany, DiceBorder2, DiceGrid2, pattern2);

                count += which;
            }
            RollDiceSound();
            await AnimateRollingDice(howmany, DiceBorder, DiceGrid, pattern);
            
            MovingPlayer = count;
            int winner = await players[playerTurn].MovePiece(count);
            if(winner == 1) {
                WinnerGame = playerTurn;
                MovingPlayer = -1;
                return;
            }
            else if(winner == 2) {
                await DisplayAlert("Must Land on 100", "Unlucky "+ players[playerTurn].name+", you rolled too large a number and went over 100. Better luck next time", "Got It");
            }
            //See if it is on a snake or ladder 
            await CheckIfLandedOnSnakeLadder();

            CheckIfMultiplePiecesOnSameSquare();
            playerTurn = (playerTurn + 1) % numberOfPlayers;
            MovingPlayer = -1;
            if (countToChangeSnakes != -1) {
                if (playerTurn == 0) {
                    --countToChangeSnakes;
                    if (countToChangeSnakes == 0) {
                        await RandomlyMoveSnakesLadders();
                        countToChangeSnakes = set.SnakesLaddersChange;
                    }
                }
            }
        }

        private async Task RandomlyMoveSnakesLadders() {
            MovingSnakesLadders = true;
            foreach (var boardpiece in snakesladdersList) {
                boardpiece.RandomMove();
            }
            await Task.Delay(2000);
            snakeshavemoved = true;
            MovingSnakesLadders = false;
        }

        private async Task CheckIfLandedOnSnakeLadder() {
            bool found = false;
            do {
                found = false;
                foreach (var boardpiece in snakesladdersList) {
                    if (boardpiece.IsStartingPlace(players[playerTurn].CurrentPosition[0], players[playerTurn].CurrentPosition[1])) {
                        await players[playerTurn].MovePieceSnakeLadder(boardpiece.EndPosition);
                        found = true;
                        break;
                    }
                }
            } while (found); //the while loop is in case you chain from a snake onto a ladder or vice-versa
        }

        private void CheckIfMultiplePiecesOnSameSquare() {
            //See if there are more than one players in this spot
            List<Player> plinpos = new List<Player>();
            plinpos.Add(players[playerTurn]);
            foreach (var player in players) {
                if (player != players[playerTurn]) {
                    if (player.CurrentPosition[0] == players[playerTurn].CurrentPosition[0] && player.CurrentPosition[1] == players[playerTurn].CurrentPosition[1]) {
                        plinpos.Add(player);
                    }
                }
            }
            if (plinpos.Count > 1) {
                for (int i = 0; i < plinpos.Count; i++) {
                    if (i % 2 == 0) {
                        if (i == 0)
                            plinpos[i].moveplayerslightly(1.0);
                        else if (i == 2)
                            plinpos[i].moveplayerslightly(2.0);
                    }
                    else {
                        if (i == 1)
                            plinpos[i].moveplayerslightly(-1.0);
                        else if (i == 3)
                            plinpos[i].moveplayerslightly(-2.0);
                    }
                }
            }
        }


        private async Task RollDiceUsingImages() {
            /*   int howmany = random.Next(4, 10);
               int which = 0;
               int last = 0;
               for(int i= 0; i < howmany; ++i) {
                   await DiceImg.RotateYTo(DiceImg.RotationY+90, DICE_DELAY / 2);
                   do {
                       which = random.Next(1, 7);
                   } while (which == last);
                   last = which;
                   DiceImg.Source = ImageSource.FromFile("dice" + which + ".png");
                   await DiceImg.RotateYTo(DiceImg.RotationY + 90, DICE_DELAY / 2);     
               }

               rollingdice = false;*/
        }

        private async void BtnDice_Clicked(object sender, EventArgs e) {
            if (WinnerGame != -1)
                return;
            if (RollingDice)
                return;

            RollingDice = true;
            await RollDiceUsingGrid();
            //await RollDiceUsingImages();
            RollingDice = false;
        }

        private int whichnumber(int row, int column) {
            //Row 0 is the top, Row 9 is the bottom
            //Want 1-10 on bottom row, 91-100 on top
            if (row % 2 == 1) {
                //Count from left to right
                int start = (9 - row) * 10 + 1;
                return start + column;
            }
            else {
                //Count from right to left
                int start = (9 - row) * 10 + 10;
                return start - column;
            }
        }

        private LayoutOptions horizontalposition(int row, int column) {
            if (row % 2 == 1) return LayoutOptions.End;
            else return LayoutOptions.Start;
        }

        private void SetUpGrid() {
            if (Preferences.Default.Get("DeviceWidth", 480.0) < 480) {
                int hello = (int)Preferences.Default.Get("DeviceWidth", 480.0) / 10;
                GridGameTable.WidthRequest = hello * 10;
                GridGameTable.HeightRequest = hello * 12;
                TopTextLbl.WidthRequest = hello * 10;
            }
            else {
                GridGameTable.WidthRequest = 480;
                GridGameTable.HeightRequest = 576;
                TopTextLbl.WidthRequest = 480;
            }

            // This is to adjust the margin on android devices to ensure the grid borders are touching
            // Maybe can remove this in .NET Maui 8
            int setmargin = (DeviceInfo.Current.Platform == DevicePlatform.Android) ? -2 : 0;                
            for (int i = 0; i < 10; ++i) {
                for (int j = 0; j < 10; ++j) {
                    Border border = new Border
                    {
                        //Stroke = Color.FromArgb("#C49B33"),
                        Stroke = new LinearGradientBrush
                        {
                            EndPoint = new Point(0, 1),
                            GradientStops = new GradientStopCollection
                            {
                                new GradientStop { Color = Colors.Orange, Offset = 0.1f },
                                new GradientStop { Color = Colors.Brown, Offset = 1.0f }
                            },
                        },
                        Margin = setmargin,
                        StrokeThickness = 2,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        Padding = new Thickness(2, 2),
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(5, 5, 5, 5)
                        },
                        Content = new Label
                        {

                            Text = whichnumber(i, j).ToString(),
                            TextColor = Colors.White,
                            FontSize = 10,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = horizontalposition(i, j),
                            VerticalOptions = LayoutOptions.Start
                        }
                    };
                    if (whichnumber(i, j) % 2 == 0)
                        border.SetDynamicResource(Border.BackgroundProperty, "GridColour1");
                    else
                        border.SetDynamicResource(Border.BackgroundProperty, "GridColour2");
                    GridGameTable.Add(border, j, i);
                }
            }
        }


        private async void Settings_Clicked(object sender, EventArgs e) {
            SettingsPage setpage = new SettingsPage(set);
            fromsettingspage = true;
            await Navigation.PushAsync(setpage);
        }

        private void RollDiceSound() {
            audioplayer.Play();
        }

        private async void NewGameButton_Clicked(object sender, EventArgs e) {
            if(WinnerGame == -1) {
                bool answer = await DisplayAlert("Are you sure?", "Are you sure you want to start a new game?" , "Yes" , "No" ) ;
                if (!answer)
                    return;
            }
            await Shell.Current.GoToAsync("//WelcomePage", true);
        }

        private void ResetPlayersForNewGame() {
            foreach (var player in players)
                player.RemoveImage();
            players.Clear();
            InitialisePlayers(Preferences.Default.Get("numberplayers", 2));
            WinnerGame = -1;
            if(snakeshavemoved)
                MakeSnakesLadders();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            if(everythingInitialised && !fromsettingspage) {
                ResetPlayersForNewGame();
            }
            else if (fromsettingspage) {
                UpdateSettings();
                fromsettingspage = false;
            }
            base.OnNavigatedTo(args);
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            if (!everythingInitialised) {
                Console.WriteLine(this.Width);
                LoadingPage = true;
                BindingContext = this;
                await Task.Delay(500);
                await InitialiseObjectVariables();
                LoadingPage = false;
                InitialisePlayers(Preferences.Default.Get("numberplayers", 2));
                WinnerGame = 0;
                WinnerGame = -1;
            }
        }
    }
}