using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class Cube
    {
        public Color color = Color.White;
        public CubeType cubeType;
        public Cube(CubeType cubeType, Color color) 
            {
            this.color = color;
            this.cubeType = cubeType;
        }

        public enum CubeType                                // 2 types of "Cube"
        {
            Empty, Solid
        }

    }
}
