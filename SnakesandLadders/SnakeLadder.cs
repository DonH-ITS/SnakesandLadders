

namespace SnakesandLadders
{
    public class SnakeLadder
    {
        private int StartRow;
        private int EndRow;
        private int StartCol;
        private int EndCol;
        private Image image;
        private bool isSnake;

        static public Grid grid;
        static public bool snakesmove;
        static public Random random;

        public int[] EndPosition {
            get
            {
                int[] pos = new int[2];
                pos[0] = EndRow;
                pos[1] = EndCol;
                return pos;
            }
        }


        // Instead of giving the row/column numbers, just give the number of the board
        // Have to convert from position to row/column which was messier than expected, maybe could be done more easily
        public SnakeLadder(int startPos, int endPos) {
            if (startPos == 100 || endPos == 100)
                return;
            if (startPos % 10 != 0)
                StartRow = 9 - (startPos / 10);
            else
                StartRow = 9 - (startPos / 10) + 1;
            if (endPos % 10 != 0)
                EndRow = 9 - (endPos / 10);
            else
                EndRow = 9 - (endPos / 10) + 1;
            if (StartRow == EndRow)
                return;
            if (startPos % 10 == 0) {
                if (StartRow % 2 == 0)
                    StartCol = 0;
                else
                    StartCol = 9;
            }
            else {
                if (StartRow % 2 == 0)
                    StartCol = 10 - (startPos % 10);
                else
                    StartCol = startPos % 10 - 1;
            }
            if (endPos % 10 == 0) {
                if (EndRow % 2 == 0)
                    EndCol = 0;
                else
                    EndCol = 9;
            }
            else {
                if (EndRow % 2 == 0)
                    EndCol = 10 - (endPos % 10);
                else
                    EndCol = endPos % 10 - 1;
            }
            if (StartRow < EndRow)
                placesnakeonboard();
            else if (StartRow > EndRow)
                placeladderonboard();
        }

        public SnakeLadder(int StartR, int EndR, int StartC, int EndC) {
            this.StartRow = StartR;
            this.StartCol = StartC;
            this.EndRow = EndR;
            this.EndCol = EndC;
            if (StartRow < EndRow)
                placesnakeonboard();
            else if(StartRow > EndRow)
                placeladderonboard();
        }

        private void CreateImage(double height, double width, string fileName) {
            image = new Image
            {
                Source = ImageSource.FromFile(fileName),
                ZIndex = 5,
                WidthRequest = width,
                HeightRequest = height,
                Aspect = Aspect.Fill
            };
            grid.Add(image);
        }

        private void placeladderonboard() {
            int width = Math.Abs(StartCol - EndCol) + 1;
            int height = Math.Abs(StartRow - EndRow) + 1;
            double step = grid.WidthRequest / 10;
            string imageFile = "ladder";
            int colour = random.Next(6);
            imageFile += colour.ToString();
            if (width == 1) {
                if (height <= 3)
                    imageFile += "1";
                else
                    imageFile += "2";
                imageFile += ".png";
                CreateImage(height * (step), step, imageFile);
            }
            else {
                //Pythagoras Theorem
                double endHeight = Math.Sqrt(width * width + height * height);
                if(endHeight <= 3)
                    imageFile += "1";
                else
                    imageFile += "2";
                imageFile += ".png";
                CreateImage(endHeight * (step-10), step-10, imageFile);
                double direction = 1.0;
                if (StartCol > EndCol)
                    direction = -1.0;
                double tan = direction * width / height;
                double radian = Math.Atan(tan);
                double degrees = radian * 180 / Math.PI;
                image.Rotation = degrees;

            }
            image.SetValue(Grid.RowProperty, EndRow);
            image.SetValue(Grid.RowSpanProperty, height);
            if (StartCol < EndCol)
                image.SetValue(Grid.ColumnProperty, StartCol);
            else
                image.SetValue(Grid.ColumnProperty, EndCol);
            image.SetValue(Grid.ColumnSpanProperty, width);
            isSnake = false;
        }

        private void placesnakeonboard() {
            int width = Math.Abs(StartCol - EndCol) + 1;
            int height = Math.Abs(StartRow - EndRow) + 1;
            double step = grid.WidthRequest / 10;
            if (height == 4 && width == 3) {
                place4x3snake(step);
            }
            else if (height == 3 && width == 2) {
                place3x2snake(step);
            }
            else {
                //Do a Straight One like a ladder
                placeothersnake(step, width, height);
            }
            isSnake = true;
            if(snakesmove)
                StartSnakeMoving();
        }

        private void place4x3snake(double step) {
            CreateImage(4 * (step - 5), 3 * (step - 5), "snake3.png");
            if (StartCol < EndCol) {
                image.SetValue(Grid.ColumnProperty, StartCol);
            }
            else {
                image.SetValue(Grid.ColumnProperty, EndCol);
                image.RotationY = 180;
            }
            image.SetValue(Grid.RowProperty, StartRow);
            image.SetValue(Grid.RowSpanProperty, 4);
            image.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void place3x2snake(double step) {
            CreateImage(3 * (step - 5), 2 * (step - 5), "snake2.png");
            if (StartCol < EndCol) {
                image.SetValue(Grid.ColumnProperty, StartCol);
            }
            else {
                image.SetValue(Grid.ColumnProperty, EndCol);
                image.RotationY = 180;
            }
            image.SetValue(Grid.RowProperty, StartRow);
            image.SetValue(Grid.RowSpanProperty, 3);
            image.SetValue(Grid.ColumnSpanProperty, 2);
        }

        private void placeothersnake(double step, int width, int height) {         
            if(width == 1) {
                CreateImage(step * height - 10, step - 10, "snake1.png");
            }
            else
            {
                //Pythagoras Theorem
                double endHeight = Math.Sqrt(width * width + height * height);
                CreateImage(endHeight * step - 20, step - 20, "snake1.png");
                double direction = 1.0;
                if (StartCol < EndCol)
                    direction = -1.0;
                double tan = direction * width / height;
                double radian = Math.Atan(tan);
                double degrees = radian * 180 / Math.PI;
                image.Rotation = degrees;
            }            
            image.SetValue(Grid.RowProperty, StartRow);
            image.SetValue(Grid.RowSpanProperty, height);
            if (StartCol < EndCol)
                image.SetValue(Grid.ColumnProperty, StartCol);
            else
                image.SetValue(Grid.ColumnProperty, EndCol);
            image.SetValue(Grid.ColumnSpanProperty, width);
        }

        public bool IsStartingPlace(int row, int col) {
            return (row == StartRow && col == StartCol);
        }

        private Animation animation;

        //The Snakes will move up and down a little bit. Use random numbers so each snake moves slightly differently
        private void StartSnakeMoving() {
            int translation = random.Next(2, 7);
            image.TranslationY = -1*translation;
            animation = new Animation{
                {0, 0.5, new Animation(v => image.TranslationY = v, -1*translation, translation) },
                {0.5, 1,new Animation(v => image.TranslationY = v, translation, -1*translation)  }
            };
            uint seconds = (uint)random.Next(1500, 6000);
            animation.Commit(image, "SimpleAnimation", 16, seconds, Easing.Linear, (v, c) => image.TranslationY = -1*translation, () => true);
        }

        private void StopSnakeMoving() {
            if(isSnake && animation != null) {
                image.AbortAnimation("SimpleAnimation");
            }
        }

        public void ChangeMovement() {
            if (!isSnake)
                return;
            if (!snakesmove)
                StopSnakeMoving();
            else
                StartSnakeMoving();
        }

        //Pick a Random Spot on the board for the snake/ladder
        public async void RandomMove() {
            int height = Math.Abs(StartRow - EndRow);
            int startR, endR, startC, endC;
            double translation = 0;
            if (!isSnake) 
            {
                endR = random.Next(0, 10 - height);
                startR = endR + height;
                translation = endR - EndRow;
            }
            else {
                if(snakesmove)
                    StopSnakeMoving();
                startR = random.Next(0, 10 - height);
                endR = startR + height;
                translation = startR - StartRow;
            }
            int width = Math.Abs(EndCol - StartCol);
            double xtranslation = 0;
            if(width == 0) {
                startC = endC = random.Next(0, 10);
                StartCol = startC;
                xtranslation = endC - EndCol;
            }
            else if (StartCol < EndCol) { 
                startC = random.Next(0, 10 - width);
                endC = startC + width;
                xtranslation = startC - StartCol;
                
            }
            else {
                endC = random.Next(0, 10 - width);
                startC = endC + width;
                xtranslation = endC - EndCol;
            }
            double yStep = grid.Height / 12;
            double xStep = grid.Width / 10;
            await image.TranslateTo(xStep*xtranslation, yStep * translation, 2000);
            image.TranslationY = 0;
            image.TranslationX = 0;
            StartRow = startR;
            EndRow = endR;
            StartCol = startC;
            EndCol = endC;
            image.SetValue(Grid.RowProperty, isSnake ? StartRow : EndRow);
            image.SetValue(Grid.ColumnProperty, StartCol < EndCol ? StartCol : EndCol);
            if (snakesmove && isSnake)
                StartSnakeMoving();
        }

        public void RedrawItem() {
            if (snakesmove && isSnake)
                StopSnakeMoving();
            grid.Remove(image);
            
        }

        public void RemoveImage() {
            if (snakesmove && isSnake)
                StopSnakeMoving();
            grid.Remove(image);
        }


    }
}
