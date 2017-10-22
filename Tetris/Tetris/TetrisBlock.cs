using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Tetris
{
    class TetrisBlock
    {
        public bool[,] shape;
        public Color color;
        public GridPos pos;
        public int type;
        public TetrisBlock(int type) 
            {
            this.type = type;
            switch (type)
            {                                                                                                                                       //Verschillende tetrisblokken met verschillende kleuren
                case 0: shape = new bool[,] { { false, true, true }, { false, true, false }, { false, true, false } }; color = Color.Red; break;
                case 1: shape = new bool[,] { { true, true, false }, { false, true, false }, { false, true, false } }; color = Color.Orange; break;
                case 2: shape = new bool[,] { { false, true, false }, { true, true, false }, { true, false, false } }; color = Color.Purple; break;
                case 3: shape = new bool[,] { { false, true, false }, { false, true, true }, { false, false, true } }; color = Color.Blue; break;
                case 4: shape = new bool[,] { { false, true, false, false }, { false, true, false, false }, { false, true, false, false }, { false, true, false, false } }; color = Color.Green; break;
                case 5: shape = new bool[,] { { false, true, false }, { true, true, true }, { false, false, false } }; color = Color.Yellow; break;

                default: shape = new bool[,] { { true, true }, { true, true } }; color = Color.LightBlue; break;
            }
            pos = new GridPos(5, 23);


        }

        public void SnapDown() 
            {
            while (!CheckCollision(new GridPos(pos.x, pos.y - 1)))
            {
                pos.y--;
            }
        }

        public void Solidify() 
            {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        Playingfield.grid[x + pos.x, y + pos.y].cubeType = Cube.CubeType.Solid;
                        Playingfield.grid[x + pos.x, y + pos.y].color = color;
                        if (y + pos.y > Playingfield.actualHeight) {
                            Game1.gamestate = Game1.GameState.GameOver;
                        }
                    }
                }
            }
        }

        public virtual bool CheckCollision(GridPos pos) 
            {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        if (pos.x + x >= Playingfield.grid.GetLength(0) || pos.x + x < 0 || pos.y + y >= Playingfield.ySize || pos.y + y <= 0)
                            return true;
                        if (Playingfield.grid[x + pos.x, y + pos.y].cubeType != Cube.CubeType.Empty)
                            return true;
                    }
                }
            }
            return false;
        }

        public virtual void Draw() 
            {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        Game1.blockRender.DrawCube(Game1.model, x + pos.x, y + pos.y, 0, color);
                    }
                }
            }
        }

        public void Rotate() 
            {
            do
            {
                bool[,] temp = new bool[shape.GetLength(0), shape.GetLength(1)];
                for (int x = 0; x < shape.GetLength(0); x++){
                    for (int y = 0; y < shape.GetLength(1); y++){
                        temp[x, y] = shape[-x + shape.GetLength(0) - 1, y];
                    }
                }
                shape = temp;
                temp = new bool[shape.GetLength(1), shape.GetLength(0)];

                for (int x = 0; x < shape.GetLength(0); x++){
                    for (int y = 0; y < shape.GetLength(1); y++){
                        temp[y, x] = shape[x, y];
                    }
                }
                shape = temp;
                for (int x = 0; x < shape.GetLength(0); x++){
                    for (int y = 0; y < shape.GetLength(1); y++){
                        if (shape[x, y]){
                            while (x + pos.x >= Playingfield.grid.GetLength(0))
                                pos.x -= 1;
                            while (x + pos.x < 0)
                                pos.x += 1;
                        }
                    }
                }
            }
            while (CheckCollision(pos));
        }
    }
}
