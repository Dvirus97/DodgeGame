using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGame
{
    internal class PowerUp : GamePiece
    {
        public PowerUp()
        {
            Shape.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/powerUp.png"));
            Speed = 0;
        }
    }
}
