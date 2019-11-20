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
            this.colorFields();
        }

        public Board(int n)
        {
            if (n <= 0)
            {
                throw new System.ArgumentException("Valid board size is > 0");
            }
            this.Size = n;
            this.Position = new string[this.Size, this.Size];
            this.colorFields();
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
            if (this.Position[x, y] == "P" || this.Position[x, y] == "p")
            {
                throw new System.ArgumentException($"({x},{y}) is already occupied by {this.Position[x, y]}");
            }

            if (color == PawnColor.White)
            {
                this.Position[x, y] = "P";
            }
            else
            {
                this.Position[x, y] = "p";
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

        public void colorFields()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this.Position[i, j] = (this.GetFieldColorAt(i, j) == FieldColor.White) ? "w" : "b";
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
    }
}
