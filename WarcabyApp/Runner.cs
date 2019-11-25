using System;

namespace WarcabyApp
{
    public class Runner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warcaby!!");

            Console.WriteLine("Current board position:");

            var board = new Board();

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
                Console.WriteLine($"Ruch #{moves}");
                Console.WriteLine(bestMove);
                board.PrintToOut(); 
                moves++;
            }
 
            /* foreach (var movesForPawn in board.NextMoves())
            {
                foreach (var move in movesForPawn.Value)
                {
                    Console.WriteLine($"{movesForPawn.Key.X}, {movesForPawn.Key.Y} => {move[0]}, {move[1]}");
                }
            } */
        }
    }
}
