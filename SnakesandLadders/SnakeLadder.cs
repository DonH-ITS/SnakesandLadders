

namespace SnakesandLadders
{
    public class SnakeLadder
    {
        private int StartRow;
        private int EndRow;
        private int StartCol;
        private int EndCol;
        private Image image;

        public int[] EndPosition {
            get
            {
                int[] pos = new int[2];
                pos[0] = EndRow;
                pos[1] = EndCol;
                return pos;
            }
        }

        public SnakeLadder(int StartR, int EndR, int StartC, int EndC, Grid grid) {
            this.StartRow = StartR;
            this.StartCol = StartC;
            this.EndRow = EndR;
            this.EndCol = EndC;
            if (StartR < EndR)
                placesnakeonboard(grid);
            else
                placeladderonboard(grid);
        }

        private void CreateImage(double height, double width, string fileName, Grid grid) {
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

        private void placeladderonboard(Grid grid) {
            Random random = new Random();
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
                CreateImage(height * (step), step, imageFile, grid);
                image.SetValue(Grid.RowProperty, EndRow);
                image.SetValue(Grid.RowSpanProperty, height);
                image.SetValue(Grid.ColumnProperty, StartCol);
                image.SetValue(Grid.ColumnSpanProperty, 1);
            }
            else {
                //Pythagoras Theorem
                double endHeight = Math.Sqrt(width * width + height * height);
                if(endHeight <= 3)
                    imageFile += "1";
                else
                    imageFile += "2";
                imageFile += ".png";
                CreateImage(endHeight * (step-10), step-10, imageFile, grid);
                double direction = 1.0;
                if (StartCol > EndCol)
                    direction = -1.0;
                double tan = direction * width / height;
                double radian = Math.Atan(tan);
                double degrees = radian * 180 / Math.PI;
                image.Rotation = degrees;
                image.SetValue(Grid.RowProperty, EndRow);
                image.SetValue(Grid.RowSpanProperty, height);
                if( StartCol < EndCol)
                    image.SetValue(Grid.ColumnProperty, StartCol);
                else
                    image.SetValue(Grid.ColumnProperty, EndCol);
                image.SetValue(Grid.ColumnSpanProperty, width);
            }
        }

        private void placesnakeonboard(Grid grid) {
            int width = Math.Abs(StartCol - EndCol) + 1;
            int height = Math.Abs(StartRow - EndRow) + 1;
            double step = grid.WidthRequest / 10;
            if(width == 1) {
                place1colsnake(grid, step, height);
            }
            else if (height == 4 && width == 3) {
                place4x3snake(grid, step);
            }
            else if (height == 3 && width == 2) {
                place3x2snake(grid, step);
            }
            else {
                //Do a Straight One like a ladder
                placeothersnake(grid, step, width, height);
            }
        }
        
        private void place1colsnake(Grid grid, double step, int height) {
            CreateImage(height * (step), (step - 5), "snake1.png", grid);
            Random random = new Random();
            int rotation = random.Next(2);
            image.SetValue(Grid.ColumnProperty, StartCol);
            image.SetValue(Grid.RowProperty, StartRow);
            image.SetValue(Grid.RowSpanProperty, height);
            image.RotationY = rotation * 180;
        }

        private void place4x3snake(Grid grid, double step) {
            CreateImage(4 * (step - 5), 3 * (step - 5), "snake3.png", grid);
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

        private void place3x2snake(Grid grid, double step) {
            CreateImage(3 * (step - 5), 2 * (step - 5), "snake2.png", grid);
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

        private void placeothersnake(Grid grid, double step, int width, int height) {
            //Pythagoras Theorem
            double endHeight = Math.Sqrt(width * width + height * height);
            CreateImage(endHeight * step - 20, step - 20, "snake1.png", grid);
            double direction = 1.0;
            if (StartCol < EndCol)
                direction = -1.0;
            double tan = direction * width / height;
            double radian = Math.Atan(tan);
            double degrees = radian * 180 / Math.PI;
            image.Rotation = degrees;
            image.SetValue(Grid.RowProperty, StartRow);
            image.SetValue(Grid.RowSpanProperty, height);
            if (StartCol < EndCol)
                image.SetValue(Grid.ColumnProperty, StartCol);
            else
                image.SetValue(Grid.ColumnProperty, EndCol);
            image.SetValue(Grid.ColumnSpanProperty, width);
        }

        public bool IsStartingPlace(int row, int col) {
            if (row == StartRow && col == StartCol)
                return true;
            else
                return false;
        }


    }
}
