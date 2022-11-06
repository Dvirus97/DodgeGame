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
        public List<GamePiece> Enemies { get; set; }
        public List<GamePiece> Power { get; set; }
        public Canvas cnvs { get; set; }
        public Board(Grid MasterGrid)
        {
            //cnvs = (Canvas)MasterGrid.FindName("cnvs");
            cnvs = MasterGrid.FindName("cnvs") as Canvas;
            Power = new List<GamePiece>();
            Enemies = new List<GamePiece>();
            for (int i = 0; i < 5; i++)
            {
                PowerUp power1 = new PowerUp();
                CreatePiece(power1);
                Power.Add(power1);
            }
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
            if (SameLocation(piece, Enemies, out GamePiece a) || SameLocation(piece, Power, out a))
            {
                CreatePiece(piece);
            }
            else
            {
                cnvs.Children.Add(piece.Shape);
            }
        }

        public bool SameLocation(GamePiece piece, List<GamePiece> gamePieces, out GamePiece target)
        {
            target = null;
            foreach (GamePiece gamePiece in gamePieces)
            {
                if (piece == gamePiece)
                {
                    continue;
                }
                if (Math.Abs(gamePiece.X - piece.X) < piece.Shape.Width * 0.65 &&
                    Math.Abs(gamePiece.Y - piece.Y) < piece.Shape.Height * 0.65)
                {
                    target = gamePiece;
                    return true;
                }
            }
            return false;
        }

        public void SetLoc(GamePiece piece)
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
