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
    internal class Enemy : GamePiece
    {
        public Enemy()
        {
            Random random = new Random();
            int a = random.Next(1, 4);
            Shape.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/cockroache"+a+".png"));
            Speed = 1;
        }
    }
}
