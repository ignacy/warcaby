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
                Console.WriteLine($"Półruch: #{moves}");
                Console.WriteLine(bestMove);
                board.PrintToOut();
                moves++;
            }

            if (board.IsGameOver())
            {
                if (board.Turn == PawnColor.Black)
                {
                    Console.WriteLine("Koniec gry, wygrały białe");
                }
                else
                {
                    Console.WriteLine("Koniec gry, wygrały czarne");
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warcaby!!");

            if (args.Length == 0)
            {
                Console.WriteLine("Brak parametrów odpalam obie symulacje");

                Console.WriteLine("Symulacja 1 predefiniowana pozycja");

                var board = new Board(6);

                board.SetPawnAt(new Field(2, 2), PawnColor.Black);
                board.SetPawnAt(new Field(2, 4), PawnColor.Black);
                board.SetPawnAt(new Field(3, 3), PawnColor.White);
                board.SetPawnAt(new Field(3, 1), PawnColor.White);
                board.SetPawnAt(new Field(4, 2), PawnColor.White);
                board.SetPawnAt(new Field(4, 0), PawnColor.White);

                Graj(board);

                Console.WriteLine("Symulacja 2 gra przeciwko sobie z tradycyjnej pozycji wyjsciowej 8x8");

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
                    var j = size-1;

                    var boardFromFile = new Board(size);
                    //boardFromFile.Turn = PawnColor.Black;

                    for (int i=0; i<size; i++) {
                        boardFromFile.Position[i, j] = fields[i].ToString();
                    }

                    while ((line = file.ReadLine()) != null)
                    {
                        fields = line.ToCharArray();
                        j -= 1;
                        for (int i=0; i<size; i++) {
                            boardFromFile.Position[i, j] = fields[i].ToString();
                        }
                    }
                    file.Close();  

                    Console.WriteLine("Pozycja wczytana z pliku");
                    boardFromFile.PrintToOut();

                    Console.WriteLine("Rozgrywam pozycje z pliku");
                    Graj(boardFromFile);

                }
                catch (IOException e)
                {
                    Console.WriteLine("Nie mozna wczytac pliku z pozycja");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }


}
