﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Tetris
{

    public class Game1 : Game
    {
        Song TetrisSong;
        SoundEffect Hit, Clear, GameOver;
        public static Random random;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D monotex;
        bool EndFall = false;
        public static BlockRender blockRender;
        TetrisBlock fallingBlock;
        public static Model model;
        public static Playingfield playingfield;
        public float falltimer = 0;
        public float inputtimer = 0;
        float shakeTimer;
        public static GameState gamestate = GameState.Game;
        public enum GameState { Game, GameOver}

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize() {
            base.Initialize();
            playingfield = new Playingfield(10, 26);
            random = new Random();
            for (int x = 0; x < playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < playingfield.grid.GetLength(1); y++)
                {
                    playingfield.grid[x, y] = new Cube(Cube.CubeType.Empty,Color.White);
                }
            }
            fallingBlock = new TetrisBlock();
        }
  
        protected override void LoadContent() {
           // backgr = Content.Load <Texture2D>("background");
            blockRender = new BlockRender(graphics);
            model = Content.Load<Model>("monocube");
            monotex = Content.Load<Texture2D>("monotex");
            TetrisSong = Content.Load<Song>("Tetris");
            MediaPlayer.Play(TetrisSong);
            MediaPlayer.IsRepeating = true;
        }

      
        protected override void UnloadContent() {
            
        }

        KeyboardState kstate = Keyboard.GetState();
        KeyboardState oldkstate;
        float Xbalance = 0;
        float Ybalance = 0;
        bool BreakFalling = false;
        double gameTimer = 0;
        protected override void Update(GameTime gameTime) {
            
            falltimer += gameTime.ElapsedGameTime.Milliseconds;
            gameTimer += gameTime.ElapsedGameTime.Milliseconds;
            oldkstate = kstate;
            kstate = Keyboard.GetState();
            


            if (gamestate == GameState.GameOver)
            {
                blockRender.camOffset = Vector2.Zero;
                blockRender.camPosition = new Vector3((float)Math.Sin(gameTimer / 1000) * 75 + blockRender.camTarget.X, blockRender.camPosition.Y, (float)Math.Cos(gameTimer / 1000) * 75 + blockRender.camTarget.Z);
            }
            else
            {
                

                if (falltimer > 200)
                {
                    if (fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x, fallingBlock.pos.y - 1)))
                    {
                        fallingBlock.Solidify();
                        playingfield.CheckForRow();
                        if (kstate.IsKeyDown(Keys.Down))
                        {
                            shakeTimer = 300;
                            BreakFalling = true;
                        }
                        fallingBlock = new TetrisBlock();
                    }
                    else
                    {
                        fallingBlock.pos.y -= 1;
                        falltimer = 0;
                    }
                }

                /*if (fallingBlock.pos.y == -18)
                {
                    EndFall = true;
                    fallingBlock.pos.y += 1;

                }
                else EndFall = false;*/


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {   
                    Xbalance += gameTime.ElapsedGameTime.Milliseconds;
                    if (!fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x + 1, fallingBlock.pos.y)))
                    {
                        if (oldkstate.IsKeyDown(Keys.Right))
                        {
                            inputtimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (inputtimer > 150)
                            {
                                fallingBlock.pos.x += 1;
                                inputtimer = 100;
                            }
                        }
                        else
                            fallingBlock.pos.x += 1;
                    }
                    /*blockRender.camPosition.X = -10;
                    fallingBlock.pos.x += 1;
                        if (fallingBlock.pos.x == 8)
                    {
                        fallingBlock.pos.x -= 1;
                    }*/

                }
                else if (oldkstate.IsKeyDown(Keys.Right))
                {
                    inputtimer = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Xbalance -= gameTime.ElapsedGameTime.Milliseconds;

                    if (!fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x - 1, fallingBlock.pos.y)))
                    {
                        if (oldkstate.IsKeyDown(Keys.Left))
                        {
                            inputtimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (inputtimer > 150)
                            {
                                fallingBlock.pos.x -= 1;
                                inputtimer = 100;
                            }
                        }
                        else
                            fallingBlock.pos.x -= 1;
                    }
                    /*blockRender.camPosition.X = -10;
                    fallingBlock.pos.x += 1;
                        if (fallingBlock.pos.x == 8)
                    {
                        fallingBlock.pos.x -= 1;
                    }*/

                }
                else if (oldkstate.IsKeyDown(Keys.Left))
                {
                    inputtimer = 0;
                }

                if (!kstate.IsKeyDown(Keys.Right) && !kstate.IsKeyDown(Keys.Left))
                {
                    Xbalance += (Xbalance > 0) ? -gameTime.ElapsedGameTime.Milliseconds : gameTime.ElapsedGameTime.Milliseconds;
                    if (Math.Abs(Xbalance) < gameTime.ElapsedGameTime.Milliseconds)
                        Xbalance = 0;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !oldkstate.IsKeyDown(Keys.Up))
                {
                    fallingBlock.Rotate();


                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !BreakFalling)
                {
                    falltimer = 200;
                    Ybalance += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (Ybalance > 0)
                        Ybalance -= gameTime.ElapsedGameTime.Milliseconds * 5;
                }
                if (BreakFalling && !kstate.IsKeyDown(Keys.Down))
                    BreakFalling = false;

                Xbalance = MathHelper.Clamp(Xbalance, -200, 200);
                Ybalance = MathHelper.Clamp(Ybalance, 0, 200);
                blockRender.camOffset.X = -Xbalance / 20;
                blockRender.camOffset.Y = Ybalance / 20;
                blockRender.camOffset.X += (float)Math.Sin(shakeTimer / 60 * Math.PI) * shakeTimer / 20;
                if (shakeTimer > 0)
                    shakeTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            base.Update(gameTime);
        }

       

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            

            for (int x = 1; x < playingfield.grid.GetLength(0); x++) {
                blockRender.DrawCube(model, x, 0, Color.Black);
            }
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
                    if (grid[x, y].cubeType == Cube.CubeType.Empty) {
                        completeRow = false;
                    }
                }
                if (completeRow) {
                    for (int x = 0; x < xSize; x++){
                        grid[x, y].cubeType = Cube.CubeType.Empty;
                    }
                    for (int d = y; d < ySize - 1; d++){
                        for (int x = 0; x < xSize; x++){
                            grid[x, d].cubeType = grid[x, d + 1].cubeType;
                            grid[x, d].color = grid[x, d + 1].color;
                        }
                    }
                    y -= 1;
                }
            }
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
        public Color color;
        public GridPos pos;
        public TetrisBlock(){
            switch (Game1.random.Next(0, 7)) {
                case 0: shape = new bool[,] { { false, true, true },{false, true, false},{false, true, false} }; color = Color.Red; break;
                case 1: shape = new bool[,] { { true, true, false }, { false, true, false }, { false, true, false } }; color = Color.Orange; break;
                case 2: shape = new bool[,] { { false, true, false }, { true, true, false }, { true, false, false } }; color = Color.Purple; break;
                case 3: shape = new bool[,] { { false, true, false }, { false, true, true }, { false, false, true } }; color = Color.Blue; break;
                case 4: shape = new bool[,] { { false, true,false,false }, { false, true,false,false }, { false, true,false,false }, { false,true,false,false} }; color = Color.Green; break;
                case 5: shape = new bool[,] { { false, true, false}, { true, true, true}, { false, false, false}}; color = Color.Yellow; break;

                default: shape = new bool[,] { { true, true }, { true, true } }; color = Color.LightBlue; break;
            }
            //shape = new bool[3, 3];
            //pos = new GridPos(Game1.random.Next(0,Game1.playingfield.xSize),0);
             pos =  new GridPos(5, 22);
            if (CheckCollision(pos)) {
                Game1.gamestate = Game1.GameState.GameOver;
            }
            /*for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    shape[x, y] = Game1.random.Next(0,2) == 0;
                }
            }*/
        }

        public void Solidify() {
            for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++){
                    if (shape[x, y]){
                        Game1.playingfield.grid[x + pos.x, y + pos.y].cubeType = Cube.CubeType.Solid;
                        Game1.playingfield.grid[x + pos.x, y + pos.y].color = color;
                    }
                }
            }
        }

        public bool CheckCollision(GridPos pos) {
            for (int x = 0; x < shape.GetLength(0); x++){
                for (int y = 0; y < shape.GetLength(1); y++){
                    if (shape[x, y])
                    {
                        if (pos.x + x >= Game1.playingfield.xSize || pos.x + x <= 0 || pos.y + y >= Game1.playingfield.ySize || pos.y + y <= 0)
                            return true;
                        if (Game1.playingfield.grid[x + pos.x, y + pos.y].cubeType != Cube.CubeType.Empty)
                            return true;
                    }
                }
            }
            return false;
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
           do
            {
                bool[,] temp = new bool[shape.GetLength(0), shape.GetLength(1)];
                for (int x = 0; x < shape.GetLength(0); x++)
                {
                    for (int y = 0; y < shape.GetLength(1); y++)
                    {
                        temp[x, y] = shape[-x + shape.GetLength(0) - 1, y];
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
                for (int x = 0; x < shape.GetLength(0); x++)
                {
                    for (int y = 0; y < shape.GetLength(1); y++)
                    {
                        if (shape[x, y]) {
                            while (x + pos.x >= Game1.playingfield.xSize)
                                pos.x -= 1;
                            while (x + pos.x <= 0)
                                pos.x += 1;
                        }
                    }
                }

            } while (CheckCollision(pos));
        }
    }
}
