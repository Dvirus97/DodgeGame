using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Logic logic { get; set; }
        int numOfEnemies;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void startBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            menuTextTbl.Text = "Welcome to Dodge Game";
            resumeBtn.IsEnabled = true;
            resumeBtn.Content = "Start";
            settingsGrid.Visibility = Visibility.Visible;

            startGrid.Visibility = Visibility.Collapsed;
            logic = new Logic(MasterGrid);
            logic.timer.ForEach(t => t.Stop());

        }

        private void loadBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            logic.LoadGame();
            resumeBtn.IsEnabled = true;
        }

        private void saveBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            logic.SaveGame();
        }

        private void restartBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            logic.Init();
            resumeBtn.IsEnabled = true;
            logic.brd.CreatEnemy(numOfEnemies);
            menuGrid.Visibility = Visibility.Collapsed;
            logic.ConnectKeyBoardEvent();

        }

        private void resumeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            menuTextTbl.Text = "";
            resumeBtn.Content = "Resume";
            logic.timer.ForEach(t => t.Start());

            logic.ConnectKeyBoardEvent();
            menuGrid.Visibility = Visibility.Collapsed;
        }

        private void goHomeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Visible;
            menuGrid.Visibility = Visibility.Collapsed;
        }

        private void settingsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Collapsed;
            settingsGrid.Visibility = Visibility.Visible;
        }

        private void SaveSetings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            logic.Init();
            logic.timer.ForEach(t => t.Stop());


            if (int.TryParse(enemyNumberTbx.Text, out numOfEnemies) && numOfEnemies > 1 && numOfEnemies <= 20)
            {
                logic.brd.CreatEnemy(numOfEnemies);

                ComboBoxItem cbi = ((ComboBoxItem)DifficultyCbx.SelectedItem);

                logic.speedDifficulty = double.Parse(cbi.Tag.ToString());
                settingsGrid.Visibility = Visibility.Collapsed;
                menuGrid.Visibility = Visibility.Visible;
                resumeBtn.Content = "Start";
            }
        }
    }
}
