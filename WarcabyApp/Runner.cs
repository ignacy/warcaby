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

            Console.WriteLine($"Turn to move {board.Turn}");
            board.PrintToOut();

            board.MakeMove(0, 2, 1, 3);

            Console.WriteLine("Current board position:");
            Console.WriteLine($"Turn to move {board.Turn}");
            board.PrintToOut();

            board.MakeMove(1, 5, 2, 4);
            Console.WriteLine("Current board position:");
            Console.WriteLine($"Turn to move {board.Turn}");
            board.PrintToOut();
        }
    }
}
