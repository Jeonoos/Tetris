using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tetris
{

    public class Game1 : Game
    {
        public static Random random;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public static Playingfield playingfield;
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
            playingfield = new Playingfield(12,20);
            random = new Random();
        }

  
        protected override void LoadContent() {
       
        }

      
        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }
    }
    public class Playingfield {
        public int xSize, ySize;
        private Cube[,] grid;

        public Playingfield(int xSize, int ySize) {
            this.xSize = xSize;
            this.ySize = ySize;
            grid = new Cube[xSize, ySize];
        }

        bool IsEmpty(GridPos pos) {
            return (GetCube(pos).cubeType == Cube.CubeType.Empty);
        }

        Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }
    }
    struct GridPos {
        public int x, y;
        public GridPos(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    class Cube {
        public enum CubeType {
            Empty, Solid
        }
        public CubeType cubeType = CubeType.Empty;

    }

    class TetrisBlock {
        bool[,] shape;
        GridPos pos;
        public TetrisBlock(){
            shape = new bool[3, 3];
            pos = new GridPos(Game1.random.Next(0,Game1.playingfield.xSize),0);

            for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    shape[x, y] = Game1.random.Next() == 0;
                }
            }
        }


    }
}
