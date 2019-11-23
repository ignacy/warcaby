using System;
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

      Assert.Equal(new Field(x, y).Color(), color);
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
    public void CanSetAPawnOnTheBoard(int x, int y, PawnColor color, string expected)
    {
      _board = new Board(4);
      _board.SetPawnAt(new Field(x, y), color);
      Assert.Equal(expected, _board.Position[x, y]);
    }

    [Theory]
    [InlineData(0, 1, PawnColor.White)]
    [InlineData(1, 2, PawnColor.Black)]
    public void RaisesErrorWhenFieldIsWhite(int x, int y, PawnColor color)
    {
      _board = new Board(4);
      Assert.Throws<System.ArgumentException>(() => _board.SetPawnAt(new Field(x, y), color));
    }

    [Fact]
    public void KnowsHowToCountScoreForTheStartingPosition()
    {
      _board = new Board(); // regular 8 by 8 with 3 lines of pawns
      Assert.Equal(_board.Score(PawnColor.White), 12);
      Assert.Equal(_board.Score(PawnColor.Black), 12);
    }

    [Fact]
    public void BuildsMoreComplicatedPositionAndCountsTheScore()
    {
      _board = new Board(6);
      _board.SetPawnAt(new Field(1, 1), PawnColor.White);
      _board.SetPawnAt(new Field(3, 1), PawnColor.White);
      _board.SetPawnAt(new Field(3, 3), PawnColor.Black);
      _board.SetPawnAt(new Field(4, 4), PawnColor.Black);
      _board.SetPawnAt(new Field(3, 5), PawnColor.Black);
      Assert.Equal(_board.Score(PawnColor.White), 2);
      Assert.Equal(_board.Score(PawnColor.Black), 3);
    }

    [Fact]
    public void IfTheFieldIsEmptyThereAreNoValidMoves() {
        _board = new Board(2);
        Assert.Equal(
            _board.MovesFor(new Field(0, 0)),
            new int[][] {
                new int[] {}
            }
        );
    }

    [Fact]
    public void GetValidMovesForAPawnInTheCorner()
    {
      _board = new Board(2);
      var _field = new Field(0, 0);
      _board.SetPawnAt(_field, PawnColor.White);
      Assert.Equal(
                   _board.MovesFor(_field),
                   new int[][] {
                     new int[] {1,1}
                   }
                   );

    }

    [Fact]
    public void GetValidMovesForAPawnInTheCenter()
    {
      _board = new Board(8);
      var _field = new Field(3, 3);
      _board.SetPawnAt(_field, PawnColor.White);
      Assert.Equal(
                   _board.MovesFor(_field),
                   new int[][] {
                     new int[] {2, 4},
                     new int[] {4, 4}
                   }
                  );
    }

    [Fact]
    public void GetValidMovesRecognizesCaptures() {
      _board = new Board(8);
      var _field = new Field(3, 3);
      _board.SetPawnAt(_field, PawnColor.White);
      _board.SetPawnAt(new Field(2, 2), PawnColor.Black);
      _board.SetPawnAt(new Field(2, 4), PawnColor.Black);
      Assert.Equal(
                   _board.MovesFor(_field),
                   new int[][] {
                     new int[] {1, 5},
                     new int[] {4, 4}
                   }
                  );
    }

    [Fact]
    public void KnowsHowToMakeAMove() {
      _board = new Board(6);
      var _field = new Field(3, 3);
      _board.SetPawnAt(_field, PawnColor.White);
      _board.SetPawnAt(new Field(2, 2), PawnColor.Black);
      _board.SetPawnAt(new Field(2, 4), PawnColor.Black);
      var nextMove = _board.MovesFor(_field)[0];

       Assert.Equal(
                   _board.rotateMatrix(),
                   new string[,] {
                     { "_", ".", "_", ".", "_", "." },
                     { ".", "_", "b", "_", ".", "_" },
                     { "_", ".", "_", "W", "_", "." },
                     { ".", "_", "b", "_", ".", "_" },
                     { "_", ".", "_", ".", "_", "." },
                     { ".", "_", ".", "_", ".", "_" },
                   }
                   );
 
      _board.MakeMove(3, 3, nextMove[0], nextMove[1]);

       Assert.Equal(
                   _board.rotateMatrix(),
                   new string[,] {
                     { "_", "W", "_", ".", "_", "." },
                     { ".", "_", ".", "_", ".", "_" },
                     { "_", ".", "_", ".", "_", "." },
                     { ".", "_", "b", "_", ".", "_" },
                     { "_", ".", "_", ".", "_", "." },
                     { ".", "_", ".", "_", ".", "_" }
                   }
                   );
    

 
    }
  }
}
