using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame
{
    internal class Player : GamePiece
    {
        TextBlock livesTbl;
        private int _lives;
        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                livesTbl.Text = "Lives : " + _lives;
            }
        }

        public Player(Grid MasterGrid)
        {
            livesTbl = (TextBlock)MasterGrid.FindName("livesTbl");
            Shape.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Oggy.png"));
            Speed = 4;
            Lives = 3;
        }
    }
}
