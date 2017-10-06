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
        Texture2D backgr;
        bool EndFall = false;
        public static BlockRender blockRender;
        TetrisBlock fallingBlock;
        public static Model model;
        public static Playingfield playingfield;
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize() {
            base.Initialize();
            playingfield = new Playingfield(12, 20);
            random = new Random();
            for (int x = 0; x < playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < playingfield.grid.GetLength(1); y++)
                {
                    playingfield.grid[x, y] = new Cube(Cube.CubeType.Empty,Color.Orange);
                }
            }
            fallingBlock = new TetrisBlock(Color.Green);
        }
  
        protected override void LoadContent() {
            backgr = Content.Load <Texture2D>("background");
            blockRender = new BlockRender(graphics);
            model = Content.Load<Model>("monocube");
        }

      
        protected override void UnloadContent() {
            
        }

        KeyboardState kstate = Keyboard.GetState();
        KeyboardState oldkstate;

        protected override void Update(GameTime gameTime)
        {
            oldkstate = kstate;
            kstate = Keyboard.GetState();
            fallingBlock.pos.y -= 1;

            if (fallingBlock.pos.y == -18)
            {
                EndFall = true;
                fallingBlock.pos.y += 1;

            }
            else EndFall = false;
           

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (EndFall == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {

                    blockRender.camPosition.X = -10;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {

                    blockRender.camPosition.X = 10;
                }
                else blockRender.camPosition.X = 0;
            }

                if (EndFall == false)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {

                    blockRender.camPosition.X = -10;
                    fallingBlock.pos.x += 1;
                        if (fallingBlock.pos.x == 8)
                    {
                        fallingBlock.pos.x -= 1;
                    }

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {

                    blockRender.camPosition.X = 10;
                    fallingBlock.pos.x -= 1;
                    if (fallingBlock.pos.x == -8)
                    {
                        fallingBlock.pos.x += 1;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !oldkstate.IsKeyDown(Keys.Up))
                {
                    fallingBlock.Rotate();


                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !oldkstate.IsKeyDown(Keys.Down))
                {
                    fallingBlock = new TetrisBlock(Color.Green);
                }
                else
                {
                    blockRender.camPosition.X = 0;
                }

                base.Update(gameTime);


            }
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            
           
            for (int x = 0; x < playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < playingfield.grid.GetLength(1); y++)
                {
                    Cube curCube = playingfield.GetCube(new GridPos(x, y));
                    if (curCube.cubeType != Cube.CubeType.Empty)
                        blockRender.DrawCube(model, x, y, curCube.color);
                }
            }
            fallingBlock.DrawShape();
            base.Draw(gameTime);
        }
    }
    public class Playingfield {
        public int xSize, ySize;
        public Cube[,] grid;

        public Playingfield(int xSize, int ySize) {
            this.xSize = xSize;
            this.ySize = ySize;
            grid = new Cube[xSize, ySize];
        }

        bool IsEmpty(GridPos pos) {
            return (GetCube(pos).cubeType == Cube.CubeType.Empty);
        }

        public Cube GetCube(GridPos pos) {
            return grid[pos.x, pos.y];
        }
    }
    public struct GridPos {
        public int x, y;
        public GridPos(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    public class Cube {
        public Color color = Color.White;
        public CubeType cubeType;
        public Cube(CubeType cubeType, Color color) {
            this.color = color;
            this.cubeType = cubeType;
        }

        public enum CubeType {
            Empty, Solid
        }

    }

    class TetrisBlock {
        bool[,] shape;
        GridPos origin;
        Color color;
        public GridPos pos;
        public TetrisBlock(Color color){
            this.color = color;
            switch (Game1.random.Next(0, 7)) {
                case 0: shape = new bool[,] { { false, true, true },{false, true, false},{false, true, false} }; break;
                case 1: shape = new bool[,] { { true, true, false }, { false, true, false }, { false, true, false } }; break;
                case 2: shape = new bool[,] { { false, true, false }, { true, true, false }, { true, false, false } }; break;
                case 3: shape = new bool[,] { { false, true, false }, { false, true, true }, { false, false, true } }; break;
                case 4: shape = new bool[,] { { false, true,false,false }, { false, true,false,false }, { false, true,false,false }, { false,true,false,false} }; break;
                case 5: shape = new bool[,] { { false, true, false}, { true, true, true}, { false, false, false}}; break;

                default: shape = new bool[,] { { true, true }, { true, true } };break;
            }
            //shape = new bool[3, 3];
            //pos = new GridPos(Game1.random.Next(0,Game1.playingfield.xSize),0);
             pos =  new GridPos(-1, 18);

            /*for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    shape[x, y] = Game1.random.Next(0,2) == 0;
                }
            }*/
        }
        public void DrawShape() {
            for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++) {
                    if (shape[x, y]) {
                        Game1.blockRender.DrawCube(Game1.model,x + pos.x,y + pos.y,color);
                    }
                }
            }
        }

        public void Rotate() {

            bool[,] temp = new bool[shape.GetLength(0), shape.GetLength(1)];
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    temp[x, y] = shape[-x + shape.GetLength(0)-1, y];
                }
            }
            shape = temp;
            temp = new bool[shape.GetLength(1), shape.GetLength(0)];

            for (int x = 0; x < shape.GetLength(0); x++)
            {
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    temp[y, x] = shape[x, y];
                }
            }
            shape = temp;
        }
    }
}
