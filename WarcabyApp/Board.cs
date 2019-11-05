namespace WarcabyApp
{
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
    }
}
