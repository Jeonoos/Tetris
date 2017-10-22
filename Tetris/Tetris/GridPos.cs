using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public struct GridPos                   //een struct om posities in een grid iets makkelijker door te geven
    {
        public int x, y;
        public GridPos(int x, int y) 
            {
            this.x = x;
            this.y = y;
        }
    }
}
