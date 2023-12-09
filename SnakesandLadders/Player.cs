
namespace SnakesandLadders
{
    public class Player
    {
        private int position;
        static public Grid grid;
        static public bool mustRoll100;
        private int row;
        private int column;
        private Image image;
        public int[] CurrentPosition
        {
            get
            {
                int[] pos = new int[2];
                pos[0] = row;
                pos[1] = column;
                return pos;
            }
        }

        public string name { get; set; }

        public Player(string name, int playerno) {
            this.name = name;
            position = 0;
            if (playerno == 1 || playerno == 3)
                row = 10;
            else
                row = 11;
            if (playerno == 1 || playerno == 2)
                column = 0;
            else
                column = 1;
            image = new Image()
            {
                Source = ImageSource.FromFile("player" + playerno + ".png"),
                ZIndex = 10
            };
            grid.Add(image, column, row);
            image.IsVisible = true;
        }

        public async Task<int> MovePiece(int amount) {
            int direction;
            double xStep = grid.Width / 10;
            double yStep = grid.Height / 12;
            image.TranslationX = 0;
            if(position + amount > 100 && mustRoll100) {
                return 2;
            }
            for (int i = 0; i < amount; ++i) {
                if(position == 0) {
                    int height = 9 - row;
                    int width = 0 - column;
                    await image.TranslateTo(width * xStep, height * yStep, 250);
                    image.TranslationX = 0;
                    image.TranslationY = 0;
                    row = 9;
                    column = 0;
                    image.SetValue(Grid.ColumnProperty, column);
                    image.SetValue(Grid.RowProperty, row);
                    position++;
                    continue;
                }
                if (row % 2 == 0)
                    direction = -1;
                else
                    direction = 1;
                if (position % 10 != 0) {
                    ++position;
                    column = column + direction;
                    await MoveHorizontally(xStep * direction);
                }
                else {
                    ++position;
                    --row;
                    await MoveVertically(yStep);
                }
                if (position == 100) { //Win the Game
                    return 1;
                }
            }
            return 0;
        }

        private async Task MoveHorizontally(double xStep) {
            await image.TranslateTo(xStep, 0, 250);
            image.TranslationX = 0;
            image.SetValue(Grid.ColumnProperty, column);
        }

        private async Task MoveVertically(double yStep) {
            await image.TranslateTo(0, -1 * yStep, 250);
            image.TranslationY = 0;
            image.SetValue(Grid.RowProperty, row);
        }

        public async Task MovePieceSnakeLadder(int[] where) {
            double xStep = grid.Width / 10;
            double yStep = grid.Height / 12;
            int height = where[0] - row;
            int width = where[1] - column;
            await image.TranslateTo(width * xStep, height * yStep, 500);
            image.TranslationX = 0;
            image.TranslationY = 0;
            row = where[0];
            column = where[1];
            position = whichnumber(row, column);
            image.SetValue(Grid.RowProperty, row);
            image.SetValue(Grid.ColumnProperty, column);
        }

        public void moveplayerslightly(double direction) {
            image.TranslationX = direction*6;
        }

        private static int whichnumber(int row, int column) {
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

        public void RemoveImage() {
            grid.Remove(image);
        }

    }
}
