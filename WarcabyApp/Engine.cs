using System;
using System.Linq;

namespace WarcabyApp
{

    public class Engine
    {
        private readonly int DEFAULT_DEPTH = 3;
        public int Depth { get; }

        public Engine()
        {
            this.Depth = DEFAULT_DEPTH;
        }

    }
}