using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Playingfield
    {
        public int xSize, ySize;
        public Cube[,] grid;

        public Playingfield(int xSize, int ySize) {
            this.xSize = xSize;
            this.ySize = ySize;
            grid = new Cube[xSize, ySize];
        }

        bool IsEmpty(GridPos pos) {
            if (pos.x > xSize || pos.y < 0 || pos.y > ySize || pos.y <= 0)
                return false;
            return (GetCube(pos).cubeType == Cube.CubeType.Empty);
        }

        public Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }

        public void CheckForRow() {
            for (int y = 0; y < ySize; y++)
            {
                bool completeRow = true;
                for (int x = 1; x < xSize; x++)
                {
                    if (grid[x, y].cubeType == Cube.CubeType.Empty)
                    {
                        completeRow = false;
                    }
                }
                if (completeRow)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        grid[x, y].cubeType = Cube.CubeType.Empty;
                    }
                    for (int d = y; d < ySize - 1; d++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            grid[x, d].cubeType = grid[x, d + 1].cubeType;
                            grid[x, d].color = grid[x, d + 1].color;
                        }
                    }
                    y -= 1;
                }
            }
        }
    }
}
