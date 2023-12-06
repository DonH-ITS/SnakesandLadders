namespace SnakesandLadders
{
    public class Player
    {
        private int position;
        static public Grid grid;
        private int row;
        private int column;
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
        private Image image;

        public Player(string name, Image image) {
            this.name = name;
            position = 1;
            row = 9;
            column = 0;
            this.image = image;
            image.IsVisible = true;
        }

        public async Task MovePiece(int amount) {
            int direction;
            double xStep = grid.Width / 10;
            double yStep = grid.Height / 12;
            for (int i = 0; i < amount; ++i) {
                if (position == 100)
                    break;
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
            }
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

    }
}
