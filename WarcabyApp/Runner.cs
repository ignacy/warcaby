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


            //Console.WriteLine($"Turn to move {board.Turn}");
            //board.PrintToOut();

            //board.MakeMove(0, 2, 1, 3);

            //Console.WriteLine("Current board position:");
            //Console.WriteLine($"Turn to move {board.Turn}");
            //board.PrintToOut();

            //board.MakeMove(1, 5, 2, 4);
            //Console.WriteLine("Current board position:");
            //Console.WriteLine($"Turn to move {board.Turn}");
            //board.PrintToOut();

            var engine = new Engine(board);
            Console.WriteLine(engine.ScoreMoves());

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
