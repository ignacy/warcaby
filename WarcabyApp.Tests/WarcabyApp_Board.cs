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
                     { ".", "_", ".", "_" },
                     { "_", ".", "_", "." },
                     { ".", "_", ".", "_" },
                     { "_", ".", "_", "." }
                         }
                         );
        }


      [Theory]
      [InlineData(0, 0, PawnColor.White, "W")]
      [InlineData(0, 0, PawnColor.Black, "b")]
      [InlineData(1, 1, PawnColor.White, "W")]
       public void CanSetAPawnOnTheBoard(int x, int y, PawnColor color, string expected) {
            _board = new Board(4);
            _board.SetPownAt(x, y, color);
            Assert.Equal(expected, _board.Position[x, y]);
      }

      [Theory]
      [InlineData(0, 1, PawnColor.White)]
      [InlineData(1, 2, PawnColor.Black)]
      public void RaisesErrorWhenFieldIsWhite(int x, int y, PawnColor color) {
        _board = new Board(4);
        Assert.Throws<System.ArgumentException>(() => _board.SetPownAt(x, y, color));
      }

      [Fact]
      public void KnowsHowToCountScoreForTheStartingPosition() {
        _board = new Board(); // regular 8 by 8 with 3 lines of pawns
        Assert.Equal(_board.Score(PawnColor.White), 12);
        Assert.Equal(_board.Score(PawnColor.Black), 12);
      }

      [Fact]
      public void BuildsMoreComplicatedPositionAndCountsTheScore() {
          _board = new Board(6);
          _board.SetPownAt(1,1, PawnColor.White);
          _board.SetPownAt(3,1, PawnColor.White);
          _board.SetPownAt(3,3, PawnColor.Black);
          _board.SetPownAt(4,4, PawnColor.Black);
          _board.SetPownAt(3,5, PawnColor.Black);
          Assert.Equal(_board.Score(PawnColor.White), 2);
          Assert.Equal(_board.Score(PawnColor.Black), 3);
      }
    }
}
