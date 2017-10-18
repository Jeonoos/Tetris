using Microsoft.Xna.Framework;
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
        Texture2D monotex, backgr, playbutton;
        public static BlockRender blockRender;
        PreviewTetrisBlock nextBlock;
        TetrisBlock fallingBlock;
        PreviewTetrisBlock savedBlock;
        GhostBlock ghostBlock;
        SpriteFont font;
        
        public static Model model;
        public static Model modeltransp;
        public static Model backgroundmodel;
        public static Model emptycube;
        public float falltimer = 0, inputtimer = 0 , shakeTimer;
        public static GameState gamestate = GameState.Menu;
        public enum GameState { Game, GameOver, Menu}
        public float[] LevelSpeeds = {600,400,300,200,150,100,75,50,25,10};
        public Color[] LevelColors = { Color.LightBlue, Color.LightGreen, Color.LightGoldenrodYellow, Color.Black, Color.White, Color.Orange, Color.Blue, Color.DarkCyan , Color.DarkRed, Color.DarkSalmon};
        public int level = 1;
        public static float Score = 0;  
        public bool UsedHold = false;
        public Game1() 
            {


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 720;
            //graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

        }

        protected override void Initialize() 
            {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
            random = new Random();
            for (int x = 0; x < Playingfield.grid.GetLength(0); x++)
            {
                for (int y = 0; y < Playingfield.grid.GetLength(1); y++)
                {
                    Playingfield.grid[x, y] = new Cube(Cube.CubeType.Empty,Color.White);
                }
            }

            nextBlock = new PreviewTetrisBlock(Game1.random.Next(0, 7));
            fallingBlock = new TetrisBlock(nextBlock.type);
            ghostBlock = new GhostBlock(fallingBlock.type, fallingBlock.pos, fallingBlock.shape);
            nextBlock = new PreviewTetrisBlock(Game1.random.Next(0, 7));
            savedBlock = null;
        }
  
        protected override void LoadContent() 
            {
            font = Content.Load<SpriteFont>("Score");
            backgr = Content.Load <Texture2D>("Backgr");
            blockRender = new BlockRender(graphics);
            model = Content.Load<Model>("monocube");
            modeltransp = Content.Load<Model>("monocubetransp");
            backgroundmodel = Content.Load<Model>("Background");
            emptycube = Content.Load<Model>("EmptyCube");
            monotex = Content.Load<Texture2D>("monotex");
            playbutton = Content.Load<Texture2D>("playbutton");
            TetrisSong = Content.Load<Song>("Tetris");
            MediaPlayer.Play(TetrisSong);
            MediaPlayer.IsRepeating = true;
        }

      
        protected override void UnloadContent() 
            {
            
        }
        KeyboardState kstate = Keyboard.GetState();
        KeyboardState oldkstate;
        float Xbalance = 0;
        float Ybalance = 0;
        bool BreakFalling = false;
        double gameTimer = 0;
        double gameOverTimer = 0;
        float groundMoveTimer = 300f;
        protected override void Update(GameTime gameTime)
        {
            {
                falltimer += gameTime.ElapsedGameTime.Milliseconds;
                gameTimer += gameTime.ElapsedGameTime.Milliseconds;
                oldkstate = kstate;
                kstate = Keyboard.GetState();
                level = MathHelper.Clamp((int)(Score / 1000), 0, LevelSpeeds.Length - 1);


                if (gamestate == GameState.Menu)
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !oldkstate.IsKeyDown(Keys.Space))
                    {
                        gamestate = GameState.Game;
                    }
                }

                else if (gamestate == GameState.Game)
                    {
                    ghostBlock = new GhostBlock(fallingBlock.type, fallingBlock.pos, fallingBlock.shape);
                    if (falltimer > LevelSpeeds[level])
                    {
                        if (fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x, fallingBlock.pos.y - 1)))
                        {
                            if (groundMoveTimer <= 0 || kstate.IsKeyDown(Keys.Down))
                            {
                                groundMoveTimer = 300f;
                                fallingBlock.Solidify();
                                Playingfield.CheckForRow();
                                if (kstate.IsKeyDown(Keys.Down))
                                {
                                    shakeTimer = 300;
                                    BreakFalling = true;
                                }
                                UsedHold = false;
                                fallingBlock = new TetrisBlock(nextBlock.type);
                                nextBlock = new PreviewTetrisBlock(Game1.random.Next(0, 7));
                            }
                            else
                            {
                                groundMoveTimer -= gameTime.ElapsedGameTime.Milliseconds;
                            }
                        }
                        else
                        {
                            fallingBlock.pos.y -= 1;
                            falltimer = 0;
                        }
                    }


                    if (Keyboard.GetState().IsKeyDown(Keys.C) && !oldkstate.IsKeyDown(Keys.C))
                    {
                        int temptype = fallingBlock.type;
                        if (!UsedHold)
                        {
                            UsedHold = true;
                            if (savedBlock != null)
                            {
                                fallingBlock = new TetrisBlock(savedBlock.type);
                                savedBlock = new PreviewTetrisBlock(temptype);
                                savedBlock.previewPosition = new GridPos(-3, 18);
                            }
                            else
                            {
                                savedBlock = new PreviewTetrisBlock(temptype);
                                fallingBlock = new TetrisBlock(nextBlock.type);
                                nextBlock = new PreviewTetrisBlock(Game1.random.Next(0, 7));
                                savedBlock.previewPosition = new GridPos(-3, 18);
                            }
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !oldkstate.IsKeyDown(Keys.Space))
                    {
                        fallingBlock.SnapDown();
                        groundMoveTimer = 0;
                        falltimer = LevelSpeeds[level];
                        shakeTimer = 200;
                    }

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
                        falltimer += gameTime.ElapsedGameTime.Milliseconds * 15;
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


                    if (gamestate == GameState.GameOver)
                    {
                        gameOverTimer += gameTime.ElapsedGameTime.Milliseconds * 0.5f;
                        blockRender.camOffset = Vector2.Zero;
                        blockRender.camPosition = new Vector3((float)Math.Sin(gameOverTimer / 1000) * 75 + blockRender.camTarget.X, blockRender.camPosition.Y, (float)Math.Cos(gameOverTimer / 1000) * 75 + blockRender.camTarget.Z);
                    }
                }
                base.Update(gameTime);
            }
        }
       

        protected override void Draw(GameTime gameTime) 
            {
            if (gamestate == GameState.Menu)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Welcome to Tetris", new Vector2(50, 20), Color.White);
                spriteBatch.Draw(playbutton, new Vector2(360, 550), Color.White);
               // spriteBatch.Draw(backgr, new Vector2 (10,10), Color.White);
                spriteBatch.End();
            }
            if (gamestate == GameState.Game)
            {
                
          
                GraphicsDevice.Clear(LevelColors[level]);
                blockRender.DrawCube(backgroundmodel, 0, 0, -20, Color.White);
                spriteBatch.Begin();
                spriteBatch.Draw(backgr,Vector2.Zero, LevelColors[level]);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Score: " + Score, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(font, "Level: " + (level += 1), new Vector2(10, 50), Color.White);
                spriteBatch.End();

                for (int x = 1; x < Playingfield.grid.GetLength(0); x++)
                {
                    blockRender.DrawCube(model, x, 0, 0, Color.Gray, 0);
                }


                for (int x = 1; x < Playingfield.grid.GetLength(0); x++)

                {
                    for (int y = 0; y < Playingfield.grid.GetLength(1); y++)
                    {
                        Cube curCube = Playingfield.GetCube(new GridPos(x, y));
                        if (curCube.cubeType != Cube.CubeType.Empty)
                            blockRender.DrawCube(model, x, y, 0, curCube.color);
                        else if (y < Playingfield.grid.GetLength(1) - 4)
                        {
                            blockRender.DrawCube(modeltransp, x, y, 0, Color.White, 0.9f);
                        }

                    }
                }
                fallingBlock.Draw();
                nextBlock.Draw();
                ghostBlock.Draw();
                if (savedBlock != null)
                    savedBlock.Draw();

                base.Draw(gameTime);
            }
        }
    }



   
}
