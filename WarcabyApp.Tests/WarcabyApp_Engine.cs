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

  }
}