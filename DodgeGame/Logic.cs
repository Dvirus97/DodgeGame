using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    internal class Logic
    {
        public Board brd { get; set; }
        public DispatcherTimer timer { get; set; }
        public Canvas cnvs { get; set; }
        public Grid MasterGrid { get; set; }
        Button resumeBtn { get; set; }
        public double speedDifficulty { get; set; }
        bool isUp, isDown, isLeft, isRight;
        TextBlock enemyCountTbl;

        public Logic(Grid MasterGrid)
        {
            this.MasterGrid = MasterGrid;
            cnvs = (Canvas)MasterGrid.FindName("cnvs");
            resumeBtn = (Button)MasterGrid.FindName("resumeBtn");
            enemyCountTbl = (TextBlock)MasterGrid.FindName("enemyCountTbl");
            Init();
        }

        public void Init()
        {
            cnvs.Children.Clear();
            brd = new Board(MasterGrid);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
            timer.Start();
            enemyCountTbl.Text = $"Enemies : {brd.Enemies.Count}";
        }

        private void Timer_Tick(object sender, object e)
        {
            PlayerMove();
            EnemyMove();
            CheckHit();
        }

        public void ConnectKeyBoardEvent()
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        private void PlayerMove()
        {
            if (isUp)
            { brd.Player.Y -= brd.Player.Speed; }
            if (isDown)
            { brd.Player.Y += brd.Player.Speed; }
            if (isLeft)
            { brd.Player.X -= brd.Player.Speed; }
            if (isRight)
            { brd.Player.X += brd.Player.Speed; }
            brd.PieceGoOut(brd.Player);
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUp = true;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = true;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = true;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = true;
                    break;
                case Windows.System.VirtualKey.Space:
                    PrintMessage("Game Paused");
                    break;
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUp = false;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = false;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = false;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = false;
                    break;
            }
        }


        private void CheckHit()
        {
            if (brd.SameLocation(brd.Player, out Enemy enemy1))
            {
                if (enemy1 != null)
                {
                    cnvs.Children.Remove(enemy1.Shape);
                    brd.Enemies.Remove(enemy1);
                    brd.Player.Lives--;
                    if (brd.Player.Lives <= 0)
                    {
                        PrintMessage("Game Over","You Lose");
                        resumeBtn.IsEnabled = false;
                    }
                    if (brd.Enemies.Count <= 1)
                    {
                        PrintMessage("Game Over","You Win");
                        resumeBtn.IsEnabled = false;
                    }
                }
            }
            else
                for (int i = 0; i < brd.Enemies.Count; i++)
                {
                    if (brd.SameLocation(brd.Enemies[i], out Enemy e))
                    {
                        cnvs.Children.Remove(brd.Enemies[i].Shape);
                        brd.Enemies.Remove(brd.Enemies[i]);
                        brd.Enemies.ForEach(enemy => enemy.Speed += speedDifficulty);
                        if (brd.Enemies.Count <= 1)
                        {
                            PrintMessage("Game Over","You Win");
                            resumeBtn.IsEnabled = false;
                        }
                    }
                }
            enemyCountTbl.Text = $"Enemies : {brd.Enemies.Count}";
        }

        private void EnemyMove()
        {
            foreach (Enemy enemy in brd.Enemies)
            {
                double goX = brd.Player.X - enemy.X;
                double goY = brd.Player.Y - enemy.Y;

                if (goX > 0)
                { enemy.X += enemy.Speed; }
                if (goX < 0)
                { enemy.X -= enemy.Speed; }
                if (goY > 0)
                { enemy.Y += enemy.Speed; }
                if (goY < 0)
                { enemy.Y -= enemy.Speed; }
            }
        }

        private void PrintMessage(string head, string text = "")
        {
            timer.Stop();
            isUp = isDown = isLeft = isRight = false;

            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;

            TextBlock menuTextTbl = (TextBlock)MasterGrid.FindName("menuTextTbl");
            Grid menuGrid = (Grid)MasterGrid.FindName("menuGrid");
            menuTextTbl.Text = head + ((text == "") ? "" : "\n"+ text);
            menuGrid.Visibility = Visibility.Visible;
        }



        public async void SaveGame()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("saveGame.txt", CreationCollisionOption.ReplaceExisting);

            foreach (Enemy enemy in brd.Enemies)
            {
                await FileIO.AppendTextAsync(sampleFile, $"{enemy.Data}{Environment.NewLine}");
            }

            await FileIO.AppendTextAsync(sampleFile, $"{brd.Player.Data}|{brd.Player.Lives}");
            PrintMessage("Game Saved","Press Resume to continue");
        }

        public async void LoadGame()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync("saveGame.txt");
            string allLines = await FileIO.ReadTextAsync(sampleFile);
            ReadFile(allLines);
            PrintMessage("Game Loaded","Press Resume to continue");
        }

        private void ReadFile(string allLines)
        {
            string[] lines = allLines.Split("\n");
            string[] line;

            cnvs.Children.Clear();
            brd.Enemies.Clear();

            for (int i = 0; i < lines.Length - 1; i++)
            {
                line = lines[i].Split("|");
                Enemy enemy1 = new Enemy();
                ReCreatePiece(enemy1, line);
                brd.Enemies.Add(enemy1);
            }

            line = lines[lines.Length - 1].Split("|");
            brd.Player.Lives = int.Parse(line[3]);
            ReCreatePiece(brd.Player, line);
        }

        void ReCreatePiece(GamePiece piece, string[] line)
        {
            piece.X = double.Parse(line[0]);
            piece.Y = double.Parse(line[1]);
            piece.Speed = double.Parse(line[2]);

            cnvs.Children.Add(piece.Shape);
        }
    }
}
