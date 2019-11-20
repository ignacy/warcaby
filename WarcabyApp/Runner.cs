using System;

namespace WarcabyApp
{
    public class Runner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var board = new Board(8);
            board.PrintToOut();
        }
    }
}
