using Xunit;
using WarcabyApp;

namespace WarcabyApp.UnitTests.Services
{
    public class WarcabyApp_Board
    {
        private Board _board;

        [Fact]
        public void DefaultBoardSize_Is8by8()
        {
            _board = new Board();
            var size = _board.Size;
            Assert.Equal(8, size);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public void BoardSizeIsAccepted(int size)
        {
            _board = new Board(size);
            var actualSize = _board.Size;
            Assert.Equal(size, actualSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void RaiseExceptionWhenWrongBoardSizeIsUsed(int size)
        {
            Assert.Throws<System.ArgumentException>(() => new Board(size));
        }


        [Theory]
        [InlineData(0, 0, FieldColor.Black)]
        [InlineData(1, 1, FieldColor.Black)]
        [InlineData(3, 1, FieldColor.Black)]
        [InlineData(3, 0, FieldColor.White)]
        [InlineData(0, 1, FieldColor.White)]
        [InlineData(7, 7, FieldColor.Black)]
        public void RecognizesBoardsFieldColorCorrectly(int x, int y, FieldColor color)
        {
            _board = new Board(8);

            Assert.Equal(_board.GetFieldColorAt(x, y), color);
        }

        [Fact]
        public void ReprezentsBoardAsArrayOfString()
        {
            _board = new Board(4);

            Assert.Equal(
                         _board.Position,
                         new string[,] {
                     { "b", "w", "b", "w" },
                     { "w", "b", "w", "b" },
                     { "b", "w", "b", "w" },
                     { "w", "b", "w", "b" }
                         }
                         );
        }


      [Theory]
      [InlineData(0, 0, PawnColor.White, "P")]
      [InlineData(0, 0, PawnColor.Black, "p")]
      [InlineData(1, 1, PawnColor.White, "P")]
       public void CanSetAPawnOnTheBoard(int x, int y, PawnColor color, string expected) {
            _board = new Board(4);
            _board.SetPownAt(x, y, color);
            Assert.Equal(expected, _board.Position[x, y]);
      }
    }
}
