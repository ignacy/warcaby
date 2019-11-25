using System;

namespace WarcabyApp
{
    public class Runner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warcaby!!");
            Console.WriteLine("Current board position:");

            var board = new Board(6);

            board.SetPawnAt(new Field(2,2), PawnColor.Black);
            board.SetPawnAt(new Field(2,4), PawnColor.Black);
            board.SetPawnAt(new Field(3,3), PawnColor.White);
            board.SetPawnAt(new Field(3,1), PawnColor.White);
            board.SetPawnAt(new Field(4,2), PawnColor.White);
            board.SetPawnAt(new Field(4,0), PawnColor.White);

            board.PrintToOut();
            int moves = 1;


            while (!board.IsGameOver() && moves < 100) {
                var engine = new Engine(board);
                var bestMove = engine.ScoreMoves();
                board = bestMove.boardAfterMove;
                Console.WriteLine($"Półruch: #{moves}");
                Console.WriteLine(bestMove);
                board.PrintToOut(); 
                moves++;
            }

            if (board.IsGameOver()) {
                if (board.Turn == PawnColor.Black) {
                    Console.WriteLine("Koniec gry, wygrały białe");
                } else {
                    Console.WriteLine("Koniec gry, wygrały czarne");
                }
            }
        }
    }
}
