
using Microsoft.Maui.Controls.Shapes;

namespace SnakesandLadders
{
    public partial class MainPage : ContentPage
    {
        Random random;
        private bool rollingdice = false;
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

        const int DICE_DELAY = 250;
        Color DICE_COLOR = Color.FromRgb(150, 195, 0);
        Player player1;

        private List<SnakeLadder> snakesladders;

        public MainPage() {
            InitializeComponent();
            SetUpGrid();
            InitRandomDice();
            InitialisePlayer();
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
            snakesladders = new();
            //  snakesladders.Add(new SnakeLadder(5, 3, 8, 3, "snake4x1.png", true, GridGameTable));
            // snakesladders.Add(new SnakeLadder(2, 2, 4, 3, "snake2x2.png", true, GridGameTable));
             snakesladders.Add(new SnakeLadder(6, 5, 4, 4, GridGameTable));
             snakesladders.Add(new SnakeLadder(7, 5, 2, 2, GridGameTable));
             snakesladders.Add(new SnakeLadder(3, 2, 3, 4, GridGameTable));
           //  snakesladders.Add(new SnakeLadder(9, 8, 6 , 5, GridGameTable));
             snakesladders.Add(new SnakeLadder(4, 0, 9, 9, GridGameTable));
             snakesladders.Add(new SnakeLadder(5, 4, 9, 7, GridGameTable));
             snakesladders.Add(new SnakeLadder(2, 4, 8, 6, GridGameTable));

            //Snakes
            snakesladders.Add(new SnakeLadder(0, 3, 4, 6, GridGameTable));
            snakesladders.Add(new SnakeLadder(3, 6, 9, 7, GridGameTable));

            snakesladders.Add(new SnakeLadder(2, 7, 6, 6, GridGameTable));
            snakesladders.Add(new SnakeLadder(1, 4, 3, 3, GridGameTable));
            snakesladders.Add(new SnakeLadder(5, 7, 4, 3, GridGameTable));
            snakesladders.Add(new SnakeLadder(7, 9, 1, 2, GridGameTable));

            snakesladders.Add(new SnakeLadder(7, 8, 6, 5, GridGameTable));
            snakesladders.Add(new SnakeLadder(0, 1, 1, 2, GridGameTable));
          //  snakesladders.Add(new SnakeLadder(2, 6, 8, 4, GridGameTable));
        }
        private void InitialisePlayer() {
            player1 = new("Donny", PlayerPiece, GridGameTable);
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
            //int howmany = random.Next(4, 10);
            int howmany = 1;
            int which = 0;
            int last = 0;
            for (int i = 0; i < howmany; ++i) {
                await DiceBorder.RotateYTo(DiceBorder.RotationY + 90, DICE_DELAY / 2);
                ClearDiceGrid(DiceGrid);
                do {
                    which = random.Next(1, 7);
                } while (which == last);
                which = 2;
                last = which;
                FillDiceGrid(which, DiceGrid);
                await DiceBorder.RotateYTo(DiceBorder.RotationY + 90, DICE_DELAY / 2);
            }
            which = 22;
            await player1.MovePiece(which);
            //See if it is on a snake or ladder 
            foreach(var boardpiece in snakesladders) {
                if (boardpiece.IsStartingPlace(player1.CurrentPosition[0], player1.CurrentPosition[1])) {
                    await player1.MovePieceSnakeLadder(boardpiece.EndPosition);
                    break;
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