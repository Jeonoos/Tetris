using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Playingfield
    {
        public static int xSize = 10, ySize = 26, yDrawSize = 22;
        public static Cube[,] grid = new Cube[xSize, ySize];

        public static Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }

        public static void CheckForRow() {
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
                if (completeRow == true)
                {
                    Game1.Score += 100;
                    for (int x = 1; x < xSize; x++)
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
