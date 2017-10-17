using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public class PreviewTetrisBlock: TetrisBlock
    {
        public PreviewTetrisBlock(int type) : base(type) {

            this.type = type;
            switch (type)
            {
                case 0: shape = new bool[,] { { false, true, true }, { false, true, false }, { false, true, false } }; color = Color.Red; break;
                case 1: shape = new bool[,] { { true, true, false }, { false, true, false }, { false, true, false } }; color = Color.Orange; break;
                case 2: shape = new bool[,] { { false, true, false }, { true, true, false }, { true, false, false } }; color = Color.Purple; break;
                case 3: shape = new bool[,] { { false, true, false }, { false, true, true }, { false, false, true } }; color = Color.Blue; break;
                case 4: shape = new bool[,] { { false, true, false, false }, { false, true, false, false }, { false, true, false, false }, { false, true, false, false } }; color = Color.Green; break;
                case 5: shape = new bool[,] { { false, true, false }, { true, true, true }, { false, false, false } }; color = Color.Yellow; break;

                default: shape = new bool[,] { { true, true }, { true, true } }; color = Color.LightBlue; break;
            }
        }

        public override void Draw() {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    if (shape[x, y])
                    {
                        Playingfield.blockRender.DrawCube(Playingfield.model, x - 3, y + +15, color);
                    }
                }
            }
        }
    }
}
