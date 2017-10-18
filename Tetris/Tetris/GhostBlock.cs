using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Tetris
{
    class GhostBlock: TetrisBlock
    {
        public GhostBlock(int type, GridPos pos, bool[,] shape) : base(type) {
            this.pos = pos;
            this.shape = shape;
            while (!CheckCollision(new GridPos(pos.x, this.pos.y - 1)))
            {
                this.pos.y--;
            }
            //color = Color.White;
        }

        public override void Draw() 
            {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        Game1.blockRender.DrawCube(Game1.modeltransp, x + pos.x, y + pos.y, 0, color, 0.6f);
                    }
                }
            }
        }
    }
}
