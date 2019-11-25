using System;
using Xunit;
using WarcabyApp;

namespace WarcabyApp.UnitTests.Services
{
  public class WarcabyApp_Engine
  {
    private Engine _engine;

    [Fact]
    public void EngineKnowsHowToHoldABoard() {
      var _board = new Board();
      _engine = new Engine(_board);

      Assert.Equal(12, _engine.StartingBoard.Score(PawnColor.Black));
    }

  }
}