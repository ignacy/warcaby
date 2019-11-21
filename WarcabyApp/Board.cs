using System;

namespace WarcabyApp
{
    public enum FieldColor { White, Black };
    public enum PawnColor { White, Black };

    public class Board
    {

        private readonly int DEFAULT_SIZE = 8;
        public int Size { get; }
        public string[,] Position { get; }

        public Board()
        {
            this.Size = DEFAULT_SIZE;
            this.Position = new string[this.Size, this.Size];
            this.ColorFields();
            this.PlaceStartingPawns(3);
        }

        public Board(int n)
        {
            if (n <= 0)
            {
                throw new System.ArgumentException("Valid board size is > 0");
            }
            this.Size = n;
            this.Position = new string[this.Size, this.Size];
            this.ColorFields();
        }

        public void SetPownAt(int x, int y, PawnColor color)
        {
            if (x > this.Size - 1)
            {
                throw new System.ArgumentException($"{x} is out of board bounds {this.Size}x{this.Size}");
            }
            if (y > this.Size - 1)
            {
                throw new System.ArgumentException($"{y} is out of board bounds {this.Size}x{this.Size}");
            }
            if (this.Position[x, y] == "W" || this.Position[x, y] == "b")
            {
                throw new System.ArgumentException($"({x},{y}) is already occupied by {this.Position[x, y]}");
            }
            if (this.GetFieldColorAt(x, y) == FieldColor.White)
            {
              throw new System.ArgumentException($"({x},{y}) is white and pawns are allowed only on black fields");
            }

            if (color == PawnColor.White)
            {
                this.Position[x, y] = "W";
            }
            else
            {
                this.Position[x, y] = "b";
            }
        }

        public FieldColor GetFieldColorAt(int x, int y)
        {
            if (x > this.Size - 1)
            {
                throw new System.ArgumentException($"{x} is out of board bounds {this.Size}x{this.Size}");
            }
            if (y > this.Size - 1)
            {
                throw new System.ArgumentException($"{y} is out of board bounds {this.Size}x{this.Size}");
            }

            if ((x % 2) != (y % 2))
            {
                return FieldColor.White;
            }
            else
            {
                return FieldColor.Black;
            }
        }

        public void ColorFields()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this.Position[i, j] = (this.GetFieldColorAt(i, j) == FieldColor.White) ? "_" : ".";
                }
            }
        }

        public void PrintToOut()
        {
            for (int i = this.Size - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    Console.Write(this.Position[i, j]);
                    Console.Write("\t");
                }

                Console.Write("\n");
            }
        }

      public int Score(PawnColor color) {
        int score = 0;

        for (int i = 0; i < this.Size; i++) {
          for (int j = 0; j < this.Size; j++) {
              if (this.Position[i, j] == "W" && color == PawnColor.White) {
                  score += 1;
              } else if (this.Position[i, j] == "b" && color == PawnColor.Black) {
                  score += 1;
              }
          }
        }

        return score;
      }

      public void PlaceStartingPawns(int rows) {
        for (int i = this.Size - 1; i >= this.Size - rows; i--) {
          for (int j = 0; j < this.Size; j++) {
            if (this.GetFieldColorAt(i, j) == FieldColor.Black) {
              this.SetPownAt(i, j, PawnColor.Black);
          }
        }
        }
        for (int i = 0; i <= rows - 1; i++) {
            for (int j = 0; j < this.Size; j++) {
              if (this.GetFieldColorAt(i, j) == FieldColor.Black) {
                this.SetPownAt(i, j, PawnColor.White);
              }
            }
          }
      }
    }
}
