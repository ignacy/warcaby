using System;
using System.Linq;

namespace WarcabyApp
{
    public enum FieldColor { White, Black };
    public enum PawnColor { White, Black };

    public class Field
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Field(int[] pair) {
            new Field(pair[0], pair[1]);
        }

        public override string ToString()
        {
            return $"({this.X},{this.Y})";
        }

        public FieldColor Color() {
            return ((this.X % 2) != (this.Y % 2)) ? FieldColor.White : FieldColor.Black;
        }
    }

    public class Board
    {
        private readonly int DEFAULT_SIZE = 8;
        public int Size { get; }
        public string[,] Position { get; set; }
        public PawnColor Turn { get; set; }

        public Board()
        {
            this.Size = DEFAULT_SIZE;
            this.Position = new string[this.Size, this.Size];
            this.ColorFields();
            this.PlaceStartingPawns(3);
            this.Turn = PawnColor.White;
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

        public void SetPawnAt(Field field, PawnColor color)
        {
            if (field.X > this.Size - 1)
            {
                throw new System.ArgumentException($"{field.X} is out of board bounds {this.Size}x{this.Size}");
            }
            if (field.Y > this.Size - 1)
            {
                throw new System.ArgumentException($"{field.Y} is out of board bounds {this.Size}x{this.Size}");
            }
            if (this.Position[field.X, field.Y] == "W" || this.Position[field.X, field.Y] == "b")
            {
                throw new System.ArgumentException($"{field} is already occupied by {this.Position[field.X, field.Y]}");
            }
            if (field.Color() == FieldColor.White)
            {
                throw new System.ArgumentException($"{field} is white and pawns are allowed only on black fields");
            }

            if (color == PawnColor.White)
            {
                this.Position[field.X, field.Y] = "W";
            }
            else
            {
                this.Position[field.X, field.Y] = "b";
            }
        }

        /**
          * Only returns valid moves for a pawn at (x,y)
          **/
        public int[][] MovesFor(Field field)
        {

            if (!this.IsInBounds(field) || !this.IsTaken(field))
            {
                return new int[][] { new int[] { } };
            }

            int[][] possible = this.NextMovesFieldsOnTheBoard(field);

            var moves = from pair in possible
                        where (
                            this.IsInBounds(new Field(pair)) &&
                            new Field(pair).Color() == FieldColor.Black &&
                            (!this.IsTaken(new Field(pair)) ||
                            (this.IsTaken(new Field(pair)) && this.CanCapture(field, pair[0], pair[1])))
                        )
                        select pair;

            foreach (var pair in moves)
            {
                if (this.CanCapture(field, pair[0], pair[1]))
                {
                    var newCoordinates = this.FindCapture(field, pair[0], pair[1]);
                    pair[0] = newCoordinates[0];
                    pair[1] = newCoordinates[1];
                }
            }

            return moves.ToArray();
        }

        /**
          * Returna all possible squares a move can be made to from (x,y)
          * Does not check game rules (field needs to be black and on the board)
          * does not care about pices, captures, etc.
          **/
        private int[][] NextMovesFieldsOnTheBoard(Field field)
        {
            int[][] possible = {
               new int[] { field.X - 1, field.Y + 1 },
               new int[] { field.X + 1, field.Y + 1 }
           };

            var moves = from pair in possible
                        where (
                            this.IsInBounds(new Field(pair)) && new Field(pair).Color() == FieldColor.Black
                        )
                        select pair;

            return moves.ToArray();
        }

        public void MakeMove(int startX, int startY, int endX, int endY)
        {
            var piece = this.Position[startX, startY];
            if (piece == "W" && this.Turn == PawnColor.Black)
            {
                throw new System.ArgumentException("Can't make the move! It's BLACKS turn");
            }
            if (piece == "b" && this.Turn == PawnColor.White)
            {
                throw new System.ArgumentException("Can't make the move! It's WHITES turn");
            }

            this.Position[startX, startY] = "."; // There was a piece here so black
            this.Position[endX, endY] = piece;

            // Handle capture
            if (endY > startY + 1)
            {
                var nextX = (endX < startX) ? (startX - 1) : (startX + 1);
                this.Position[nextX, startY + 1] = ".";
            }

            if (this.Turn == PawnColor.White)
            {
                this.Turn = PawnColor.Black;
            }
            else
            {
                this.Turn = PawnColor.White;
            }
        }

        private void ColorFields()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this.Position[i, j] = (new Field(i, j).Color() == FieldColor.White) ? "_" : ".";
                }
            }
        }

        public string[,] rotateMatrix()
        {
            var reversed = new string[this.Size, this.Size];
            var N = this.Size;
            for (int x = 0; x < N / 2; x++)
            {
                for (int y = x; y < N - x - 1; y++)
                {
                    var temp = this.Position[x, y];
                    reversed[x, y] = this.Position[y, N - 1 - x];
                    reversed[y, N - 1 - x] = this.Position[N - 1 - x,
                                            N - 1 - y];
                    reversed[N - 1 - x,
                        N - 1 - y] = this.Position[N - 1 - y, x];
                    reversed[N - 1 - y, x] = temp;
                }
            }

            return reversed;
        }
        public void PrintToOut(bool showCoordinates = false)
        {
            var rotated = this.rotateMatrix();
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {

                    if (showCoordinates)
                    {
                        Console.Write($"{i},{j} = {rotated[i, j]}");
                    }
                    else
                    {
                        Console.Write(rotated[i, j]);
                    }

                    Console.Write("\t");
                }

                Console.Write("\n");
            }
        }

        public int Score(PawnColor color)
        {
            int score = 0;

            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    if (this.Position[i, j] == "W" && color == PawnColor.White)
                    {
                        score += 1;
                    }
                    else if (this.Position[i, j] == "b" && color == PawnColor.Black)
                    {
                        score += 1;
                    }
                }
            }

            return score;
        }

        private bool IsInBounds(Field field)
        {
            return (field.X >= 0 && field.X < this.Size) && (field.Y >= 0 && field.Y < this.Size);
        }

        private bool IsTaken(Field field)
        {
            return this.Position[field.X, field.Y] == "W" || this.Position[field.X, field.Y] == "b";
        }

        private int[] FindCapture(Field field, int ox, int oy)
        {
            var oField = new Field(ox, oy);
            if (!this.IsInBounds(oField) || !this.IsTaken(oField))
            {
                return new int[] { };
            }

            var possbile = this.NextMovesFieldsOnTheBoard(oField);
            var possibleFromOXOY = from pair in possbile
                                   where (
                                       (pair[0] != field.X && pair[1] != field.Y) && (pair[1] > oy) && (pair[0] != field.X)
                                   )
                                   select pair;

            var asArray = possibleFromOXOY.ToArray();
            if (asArray.Length == 0)
            {
                return new int[] { };
            }
            else
            {
                return asArray[0];
            }
        }

        private bool CanCapture(Field field, int ox, int oy)
        {
            var oField = new Field(ox, oy);
            if (!this.IsInBounds(oField) || !this.IsTaken(oField))
            {
                return false;
            }
            return this.FindCapture(field, ox, oy).Length > 0;
        }

        /**
          * rows - how many rows are occupied by pawns [ in 8 x 8 case there are 3 rows of pawns]
          **/
        private void PlaceStartingPawns(int rows)
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = this.Size - 1; j >= this.Size - rows; j--)
                {
                    var field = new Field(i, j);
                    if (field.Color() == FieldColor.Black)
                    {
                        this.SetPawnAt(field, PawnColor.Black);
                    }
                }
            }
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var field = new Field(i, j);
                    if (field.Color() == FieldColor.Black)
                    {
                        this.SetPawnAt(field, PawnColor.White);
                    }
                }
            }
        }
    }
}
