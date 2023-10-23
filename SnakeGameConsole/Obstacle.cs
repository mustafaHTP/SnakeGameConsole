using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameConsole
{
    internal struct Obstacle
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Obstacle(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
