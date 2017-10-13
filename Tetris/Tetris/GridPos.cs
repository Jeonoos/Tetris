using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public struct GridPos
    {
        public int x, y;
        public GridPos(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}
