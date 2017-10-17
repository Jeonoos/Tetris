using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Tetris
{
    public static class Playingfield
    {
        public static int xSize = 10, ySize = 26;
        public static Cube[,] grid;
        public static BlockRender blockRender;
        public static GraphicsDeviceManager graphics;
        public static Random random;

        public static Model model;
        public static Model model2;
        public static Texture2D monotex;
        public static Song TetrisSong;
        public static SoundEffect Hit, Clear, GameOver;

        public static PreviewTetrisBlock nextBlock;
        public static TetrisBlock fallingBlock;

        public static float shakeTimer;

        public static void Initialize(Game game) {
            grid = new Cube[xSize, ySize];
            blockRender = new BlockRender(graphics);

            random = new Random();
            for (int x = 0; x < Playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < Playingfield.grid.GetLength(1); y++)
                {
                    Playingfield.grid[x, y] = new Cube(Cube.CubeType.Empty, Color.White);
                }
            }

            nextBlock = new PreviewTetrisBlock(random.Next(0, 7));
            fallingBlock = new TetrisBlock(nextBlock.type);
            nextBlock = new PreviewTetrisBlock(random.Next(0, 7));
        }

        public static  void LoadContent(Game game) {
            //backgr = Content.Load <Texture2D>("background");
            model = game.Content.Load<Model>("monocube");
            monotex = game.Content.Load<Texture2D>("monotex");
            TetrisSong = game.Content.Load<Song>("Tetris");
            MediaPlayer.Play(TetrisSong);
            MediaPlayer.IsRepeating = true;
        }

        public static bool IsEmpty(GridPos pos) {
            if (pos.x > xSize || pos.y < 0 || pos.y > ySize || pos.y <= 0)
                return false;
            return (GetCube(pos).cubeType == Cube.CubeType.Empty);
        }

        public static Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }

        public static void Draw(GameTime gameTime, Game game) {

            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            for (int x = 1; x < Playingfield.grid.GetLength(0); x++)
            {
                blockRender.DrawCube(model, x, 0, Color.Black);
            }
            for (int x = 0; x < Playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < Playingfield.grid.GetLength(1); y++)
                {
                    Cube curCube = Playingfield.GetCube(new GridPos(x, y));
                    if (curCube.cubeType != Cube.CubeType.Empty)
                        blockRender.DrawCube(model, x, y, curCube.color);

                }
            }
            fallingBlock.Draw();
            nextBlock.Draw();
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

            fallingBlock = new TetrisBlock(Playingfield.nextBlock.type);
            nextBlock = new PreviewTetrisBlock(random.Next(0, 7));
        }
    }
}
