using System;
using Xunit;
using WarcabyApp;

namespace WarcabyApp.UnitTests.Services
{
  public class WarcabyApp_Engine
  {
    private Engine _engine;

    [Fact]
    public void DefaultEngineDepthIs3()
    {
      _engine = new Engine();
      Assert.Equal(3, _engine.Depth);
    }

    [Fact]
    public void EngineKnowsHowToHoldABoard() {
      _engine = new Engine();
      var _board = new Board();

      _engine.StartingBoard = _board;

      Assert.Equal(
        12, _engine.StartingBoard.Score(PawnColor.Black)
      );

    }

  }
}