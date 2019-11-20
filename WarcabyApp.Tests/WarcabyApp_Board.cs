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


    }
}
