using System;
using System.IO;

namespace WarcabyApp
{
    public class Runner
    {
        static void Graj(Board board)
        {
            int moves = 1;

            while (!board.IsGameOver() && moves < 100)
            {
                var engine = new Engine(board);
                var bestMove = engine.ScoreMoves();
                board = bestMove.boardAfterMove;
                Console.WriteLine($"Ply: #{moves}");
                Console.WriteLine(bestMove);
                board.PrintToOut();
                moves++;
            }

            if (board.IsGameOver())
            {
                if (board.Turn == PawnColor.Black)
                {
                    Console.WriteLine("Game over. White won");
                }
                else
                {
                    Console.WriteLine("Game over. Black won");
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warcaby!!");

            if (args.Length == 0)
            {
                Console.WriteLine("No params. Running 2 simulations");

                Console.WriteLine("Simulation #1: Predefined position");

                var board = new Board(6);

                board.SetPawnAt(new Field(2, 2), PawnColor.Black);
                board.SetPawnAt(new Field(2, 4), PawnColor.Black);
                board.SetPawnAt(new Field(3, 3), PawnColor.White);
                board.SetPawnAt(new Field(3, 1), PawnColor.White);
                board.SetPawnAt(new Field(4, 2), PawnColor.White);
                board.SetPawnAt(new Field(4, 0), PawnColor.White);
                Graj(board);

                Console.WriteLine("Simulation #2: Play against self from regular 8x8 position");

                board = new Board();
                Graj(board);
            }
            else
            {
                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(@$"{args[0]}");

                    string line = file.ReadLine();

                    var fields = line.ToCharArray();
                    var size = fields.Length;
                    var j = size - 1;

                    var boardFromFile = new Board(size);
                    //boardFromFile.Turn = PawnColor.Black;

                    for (int i = 0; i < size; i++)
                    {
                        boardFromFile.Position[i, j] = fields[i].ToString();
                    }

                    while ((line = file.ReadLine()) != null)
                    {
                        fields = line.ToCharArray();
                        j -= 1;
                        for (int i = 0; i < size; i++)
                        {
                            boardFromFile.Position[i, j] = fields[i].ToString();
                        }
                    }
                    file.Close();

                    Console.WriteLine("Position loaded from the file.");
                    boardFromFile.PrintToOut();

                    Console.WriteLine("Playing position from file.");
                    Graj(boardFromFile);

                }
                catch (IOException e)
                {
                    Console.WriteLine("Error while reading the file");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }


}
