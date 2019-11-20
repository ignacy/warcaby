using System;

namespace WarcabyApp
{
    public class Runner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warcaby!!");

            Console.WriteLine("Current board position:");

            var board = new Board(8);
            board.PrintToOut();
        }
    }
}
