namespace SnakesandLadders
{
    public class Player
    {
        public int position { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public string name { get; set; }
        public Image image { get; set; }

        public Player(string name, Image image) {
            this.name = name;
            position = 1;
            row = 9;
            column = 0;
            this.image = image;
        }

        public async Task MoveHorizontally(double xStep) {
            await image.TranslateTo(xStep, 0, 250);
            image.TranslationX = 0;
            image.SetValue(Grid.ColumnProperty, column);
        }

        public async Task MoveVertically(double yStep) {
            await image.TranslateTo(0, -1 * yStep, 250);
            image.TranslationY = 0;
            image.SetValue(Grid.RowProperty, row);
        }
    }
}
