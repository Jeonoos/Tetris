using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Tetris
{
    class PreviewTetrisBlock: TetrisBlock                     //een preview blok die afzonderlijk kan worden gerenderd
    {
        public GridPos previewPosition = new GridPos(13, 18);
        public PreviewTetrisBlock(int type) : base(type) 
            {

        }

        public override void Draw() 
            {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        Game1.blockRender.DrawCube(Game1.model, x + previewPosition.x, y + previewPosition.y, -3, color);
                    }
                }
            }
        }
    }
}
