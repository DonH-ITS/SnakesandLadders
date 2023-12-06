
using Microsoft.Maui.Controls.Shapes;

namespace SnakesandLadders
{
    public partial class MainPage : ContentPage
    {
        Random random;
        private bool rollingdice = false;
        private List<SnakeLadder> snakesladdersList;
        const int DICE_DELAY = 250;
        Color DICE_COLOR = Color.FromRgb(150, 195, 0);
        private List<Player> players;
        private int playerTurn;
        private int numberOfPlayers;
        public bool Rollingdice
        {
            get => rollingdice;
            set
            {
                if(rollingdice == value) return;
                rollingdice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotRollingDice));
            }
        }
        public bool NotRollingDice => !Rollingdice;
        public MainPage() {
            InitializeComponent();
            SetUpGrid();
            InitRandomDice();
            InitialisePlayers(2);
            MakeSnakesLadders();
            BindingContext = this;
            this.LayoutChanged += OnWindowChange;
        }

        private void OnWindowChange(object sender, EventArgs e) {
            if (this.Width <= 0)
                return;
            if(this.Width < 480) {
                int newdim = (int)this.Width / 10;
                double rescale = (double)newdim * 10 / 480;
                GridGameTable.Scale = rescale;
            }
        }

        private void MakeSnakesLadders() {
            SnakeLadder.grid = GridGameTable;
            snakesladdersList = new();
           
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


        }
        private void InitialisePlayers(int howmany) {
            Player.grid = GridGameTable;
            players = new List<Player>();
            Image plyimg = null;
            for(int i=0; i<howmany; i++) {
                switch (i) {
                    case 0:
                        plyimg = Player1Piece;
                        break;
                    case 1:
                        plyimg = Player2Piece;
                        break;
                }
                players.Add(new Player("Donny", plyimg));
            }
            numberOfPlayers = howmany;
        }

        private void InitRandomDice() {
            random = new();
            FillDiceGrid(random.Next(1,7), DiceGrid);
        }

        private Ellipse drawcircle() {
            Ellipse ell = new Ellipse()
            {
                Fill = DICE_COLOR,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
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

        private async Task RollDiceUsingGrid() {
            int howmany = random.Next(4, 10);
            int which = 0;
            int last = 0;
            int DiceBorderCurrentCol = Int32.Parse(DiceBorder.GetValue(Grid.ColumnProperty).ToString());
            int DiceBorderSetCol = 0;
            double xStep = GridGameTable.Width / 10;
            if ( DiceBorderCurrentCol == 1){ 
                DiceBorder.TranslateTo(xStep * 5.5, 0, (uint)howmany * DICE_DELAY);
                DiceBorderSetCol = 6;
            }
            else {
                DiceBorder.TranslateTo(-xStep * 5.5, 0, (uint)howmany * DICE_DELAY);
                DiceBorderSetCol = 1;
            }
            DiceBorder.RotationY =  0;
            for (int i = 0; i < howmany; ++i) {
                await DiceBorder.RotateYTo(DiceBorder.RotationY + 90, DICE_DELAY / 2);
                ClearDiceGrid(DiceGrid);
                do {
                    which = random.Next(1, 7);
                } while (which == last);
                last = which;
                FillDiceGrid(which, DiceGrid);
                await DiceBorder.RotateYTo(DiceBorder.RotationY + 90, DICE_DELAY / 2);
            }
            DiceBorder.TranslationX = 0;
            DiceBorder.SetValue(Grid.ColumnProperty, DiceBorderSetCol);
            await players[playerTurn].MovePiece(which);
            //See if it is on a snake or ladder 
            foreach (var boardpiece in snakesladdersList) {
                if (boardpiece.IsStartingPlace(players[playerTurn].CurrentPosition[0], players[playerTurn].CurrentPosition[1])) {
                    await players[playerTurn].MovePieceSnakeLadder(boardpiece.EndPosition);
                    break;
                }
            }
            playerTurn = (playerTurn + 1) % numberOfPlayers;
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
            if (Rollingdice)
                return;
            Rollingdice = true;
            await RollDiceUsingGrid();
            //await RollDiceUsingImages();
            Rollingdice = false;
        }

        private int whichnumber(int row, int column) {
            //Row 0 is the top, Row 9 is the bottom
            //Want 1-10 on bottom row, 91-100 on top
            if( row % 2 == 1 ) {
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
                        Background = Color.FromArgb("#2B0B98"),
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

                            Text = whichnumber(i,j).ToString(),
                            TextColor = Colors.White,
                            FontSize = 10,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = horizontalposition(i,j),
                            VerticalOptions = LayoutOptions.Start
                        }
                    };
                    GridGameTable.Add(border, j, i);
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {

        }
    }
}