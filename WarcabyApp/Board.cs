using System;

namespace WarcabyApp
{
  public enum FieldColor { White, Black };

  public class Board
  {

    private readonly int DEFAULT_SIZE = 8;
    public int Size { get; }

    public Board()
    {
      this.Size = DEFAULT_SIZE;
    }

    public Board(int n)
    {
      if (n <= 0)
      {
        throw new System.ArgumentException("Valid board size is > 0");
      }
      this.Size = n;
    }

    public FieldColor GetFieldColorAt(int x, int y)
    {
      if (x > this.Size - 1) {
        throw new System.ArgumentException($"{x} is out of board bounds {this.Size}x{this.Size}");
      }
      if (y > this.Size - 1) {
        throw new System.ArgumentException($"{y} is out of board bounds {this.Size}x{this.Size}");
      }

      if ((x % 2) != (y % 2)) {
        return FieldColor.White;
      } else {
        return FieldColor.Black;
      }

    }

    public string[,] ToArray() {
      string[,] position = new string[this.Size, this.Size];

      for (int i = 0; i < this.Size; i++) {
        for (int j = 0; j < this.Size; j++) {
          position[i, j] = (this.GetFieldColorAt(i, j) == FieldColor.White) ? "w" : "b";
        }
      }

      return position;
    }

    public void PrintToOut() {
      string[,] position = this.ToArray();

      for (int i = this.Size - 1; i >= 0; i--) {
        for (int j = 0; j < this.Size; j++) {
          Console.Write(position[i,j]);
          Console.Write("\t");
        }

        Console.Write("\n");
      }
    }
  }
}
