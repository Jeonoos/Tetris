using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class Playingfield
    {                                                                                               //Grid
        public static int xSize = 12, ySize = 27, actualHeight = 20;                                //Grid width and height
        public static Cube[,] grid = new Cube[xSize, ySize];

        public static Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }

        public static void CheckForRow() {                                                          //Kijkt of er een rij is gemaakt
            for (int y = 0; y < ySize; y++)
            {
                bool completeRow = true;
                for (int x = 0; x < xSize; x++)
                {
                    if (grid[x, y].cubeType == Cube.CubeType.Empty)
                    {
                        completeRow = false;
                    }
                }
                if (completeRow == true)                                                            //Als dit true is, worden de blokjes verwijderd en krijg je 100 punten.
                {
                    Game1.Clearsound.Play();    
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
