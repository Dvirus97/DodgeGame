using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    internal class Board
    {
        public Player Player { get; set; }
        public List<Enemy> Enemies { get; set; }
        public Canvas cnvs { get; set; }
        public Board(Grid MasterGrid)
        {
            cnvs = (Canvas)MasterGrid.FindName("cnvs");
            Enemies = new List<Enemy>();
            Player = new Player(MasterGrid);
            CreatePiece(Player);
        }
        public void CreatEnemy(int nunberOfEnemies)
        {
            for (int i = 0; i < nunberOfEnemies; i++)
            {
                Enemy enemy1 = new Enemy();
                CreatePiece(enemy1);
                Enemies.Add(enemy1);
            }
        }
        private void CreatePiece(GamePiece piece)
        {
            SetLoc(piece);
            if (SameLocation(piece, out Enemy e))
            {
                CreatePiece(piece);
            }
            else
            {
                cnvs.Children.Add(piece.Shape);
            }
        }

        public bool SameLocation(GamePiece piece, out Enemy enemy1)
        {
            enemy1 = null;
            foreach (Enemy enemy in Enemies)
            {
                if (piece == enemy)
                {
                    continue;
                }
                if (Math.Abs(enemy.X - piece.X) < piece.Shape.Width * 0.65 &&
                    Math.Abs(enemy.Y - piece.Y) < piece.Shape.Height * 0.65)
                {
                    enemy1 = enemy;
                    return true;
                }
            }
            return false;
        }

        private void SetLoc(GamePiece piece)
        {
            Random rnd = new Random();

            piece.X = rnd.Next((int)(cnvs.ActualWidth - piece.Shape.Width));
            piece.Y = rnd.Next((int)(cnvs.ActualHeight - piece.Shape.Height));
        }


        public void PieceGoOut(GamePiece piece)
        {
            if (piece.X < 0)
            { piece.X = 0; }
            if (piece.X > cnvs.ActualWidth - piece.Shape.Width)
            { piece.X = cnvs.ActualWidth - piece.Shape.Width; }

            if (piece.Y < 0)
            { piece.Y = 0; }
            if (piece.Y > cnvs.ActualHeight - piece.Shape.Height)
            { piece.Y = cnvs.ActualHeight - piece.Shape.Height; }
        }
    }
}
