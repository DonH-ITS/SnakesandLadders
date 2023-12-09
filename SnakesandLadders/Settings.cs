
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage;
using System.Text.Json;

namespace SnakesandLadders
{
    public class Settings : INotifyPropertyChanged
    {
        private bool twodice;
        private string grid_colour1;
        private string grid_colour2;
        private string dice_colourfg;
        private string dice_colourbg;
        private bool move_snakes;
        private int snakesladderschange;
        private bool finishon100;

        public bool Finish100
        {
            get => finishon100;
            set
            {
                if (finishon100 != value) {
                    finishon100 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SnakesLaddersChange
        {
            get => snakesladderschange;
            set
            {
                if (snakesladderschange != value) {
                    snakesladderschange = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool MoveSnakes
        {
            get => move_snakes;
            set
            {
                if (move_snakes != value) {
                    move_snakes = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GRID_COLOUR1
        {
            get => grid_colour1;
            set
            {
                if(grid_colour1 != value) {
                    grid_colour1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GRID_COLOUR2
        {
            get => grid_colour2;
            set
            {
                if (grid_colour2 != value) {
                    grid_colour2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string DICE_COLOURFG
        {
            get => dice_colourfg;
            set
            {
                if (dice_colourfg != value) {
                    dice_colourfg = value;
                    OnPropertyChanged();
                }
            }
        }
        public string DICE_COLOURBG
        {
            get => dice_colourbg;
            set
            {
                if (dice_colourbg != value) {
                    dice_colourbg = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool TwoDice
        {
            get => twodice;
            set
            {
                if (value == twodice) {
                    return;                
                }
                twodice = value;
                OnPropertyChanged();
            }
        }

        public Settings() {
            //Have Default Settings in the Constructor
            TwoDice = true;
            GRID_COLOUR1 = "#2B0B98";
            GRID_COLOUR2 = "#2B0B98";
            DICE_COLOURBG = "#FFFFFF";
            DICE_COLOURFG = "#000000";
            MoveSnakes = true;
            SnakesLaddersChange = 0;
            Finish100 = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveJson() {
            string jsonstring = JsonSerializer.Serialize(this);
            string filename = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "settings.json");
            using (StreamWriter writer = new StreamWriter(filename)) {
                writer.Write(jsonstring);
            }
        }
    }
}
