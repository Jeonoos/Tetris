﻿//Tetris gamemade by Alperen Elver 6321461  and Jeroen Vreugdenhil 6211046

//All Loaded Content is Original content, except Backgr.png "https://static1.squarespace.com/static/51be3e56e4b09edc5f81e74c/t/54ab723fe4b01142027aaba0/1420522056098/?format=750w"




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
        public static SoundEffect Hitsound, Clearsound;
        public static Random random;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D monotex, backgr, playbutton, Gameoverdim, StartGame, Introscherm;
        public static BlockRender blockRender;
        PreviewTetrisBlock nextBlock, savedBlock;
        TetrisBlock fallingBlock;
        GhostBlock ghostBlock;
        SpriteFont font, font2, smallText;
        
        public static Model model ,backgroundmodel ,emptycube;
        public float falltimer = 0, inputtimer = 0 , shakeTimer;
        public static GameState gamestate = GameState.Menu;
        public enum GameState { Game, GameOver, Menu, Paused}
        public float[] LevelSpeeds = {500,400,300,200,150,100,75,50,25,18};
        public Color[] LevelColors = { Color.LightBlue, Color.LightGreen, Color.LightGoldenrodYellow, Color.Black, Color.White, Color.Orange, Color.Blue, Color.DarkCyan , Color.DarkRed, Color.DarkSalmon};

        public int level = 0;
        public static float Score = 0;  
        public bool UsedHold = false;

        KeyboardState kstate = Keyboard.GetState();
        KeyboardState oldkstate;
        float Xbalance = 0;
        float Ybalance = 0;
        float groundMoveTimer = 300f;
        double gameTimer = 0;
        double gameOverTimer = 0;
        bool BreakFalling = false;


        public Game1() 
            {


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 720;
            //graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

        }

        protected override void Initialize()                                                                //Initialize
            {
            Score = 0;
            falltimer = 0;
            inputtimer = 0;
            UsedHold = false;
            gameOverTimer = 0;
            gameTimer = 0;
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
        public float volume = 0.2f;
  
        protected override void LoadContent()                                                               //LoadContent                                      
            {

            font = Content.Load<SpriteFont>("Score");
            font2 = Content.Load<SpriteFont>("Score2");
            smallText = Content.Load<SpriteFont>("SmallText");

            backgr = Content.Load <Texture2D>("Backgr");
            StartGame = Content.Load<Texture2D>("startGame");
            Gameoverdim = Content.Load<Texture2D>("Gameoverdim");
            monotex = Content.Load<Texture2D>("monotex");
            playbutton =  Content.Load<Texture2D>("playbutton");
            Introscherm = Content.Load<Texture2D>("Introscherm");

            TetrisSong = Content.Load<Song>("Tetris");
            Hitsound = Content.Load<SoundEffect>("Hit");
            Clearsound = Content.Load<SoundEffect>("Clear");

            emptycube = Content.Load<Model>("EmptyCube");
            model = Content.Load<Model>("monocube");
            blockRender = new BlockRender(graphics);

            MediaPlayer.Play(TetrisSong);
            MediaPlayer.Volume = (volume);
            MediaPlayer.IsRepeating = true;
            }

      
        protected override void UnloadContent() 
            {
            }



        protected override void Update(GameTime gameTime)                                                                   //Update
        {
            if (Keyboard.GetState().IsKeyDown(Keys.L))    //Increase sound
                MediaPlayer.Volume += 0.005f;

            if (Keyboard.GetState().IsKeyDown(Keys.K))    //Decrease sound
                MediaPlayer.Volume -= 0.005f;

            if (Keyboard.GetState().IsKeyDown(Keys.M))    //mute sound
                MediaPlayer.Volume = 0;

            oldkstate = kstate;
            kstate = Keyboard.GetState();

            switch (gamestate)
            {
                case GameState.Paused:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !oldkstate.IsKeyDown(Keys.Escape))
                        gamestate = GameState.Menu;
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !oldkstate.IsKeyDown(Keys.Space))
                        gamestate = GameState.Game;
                    break;
                case GameState.GameOver:                                                //Alles wat tijdens "GameOver" gebeurt

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !oldkstate.IsKeyDown(Keys.Escape))
                        gamestate = GameState.Menu;
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !oldkstate.IsKeyDown(Keys.Space))
                    {
                        Initialize();
                        gamestate = GameState.Game;
                    }


                    gameOverTimer += gameTime.ElapsedGameTime.Milliseconds * 0.5f;
                    blockRender.camOffset = Vector2.Zero;
                    blockRender.camPosition = new Vector3((float)Math.Sin(gameOverTimer / 1000) * 75 + blockRender.camTarget.X, blockRender.camPosition.Y, (float)Math.Cos(gameOverTimer / 1000) * 75 + blockRender.camTarget.Z);
                    break;

                case GameState.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !oldkstate.IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !oldkstate.IsKeyDown(Keys.Space))
                    {
                        Initialize();
                        gamestate = GameState.Game;
                    }

                    break;
                case GameState.Game:
                    falltimer += gameTime.ElapsedGameTime.Milliseconds;
                    gameTimer += gameTime.ElapsedGameTime.Milliseconds;
                    level = MathHelper.Clamp((int)Math.Floor(Score / 1000), 0, LevelSpeeds.Length - 1);


                    

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) //pause
                        gamestate = GameState.Paused;



                    ghostBlock = new GhostBlock(fallingBlock.type, fallingBlock.pos, fallingBlock.shape);
                    if (falltimer > LevelSpeeds[level])
                    {
                        if (fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x, fallingBlock.pos.y - 1)))
                        {

                            if (groundMoveTimer <= 0 || kstate.IsKeyDown(Keys.Down))
                            {
                                Hitsound.Play();
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
                                savedBlock.previewPosition = new GridPos(-4, 18);
                            }
                            else
                            {
                                savedBlock = new PreviewTetrisBlock(temptype);
                                fallingBlock = new TetrisBlock(nextBlock.type);
                                nextBlock = new PreviewTetrisBlock(Game1.random.Next(0, 7));
                                savedBlock.previewPosition = new GridPos(-4, 18);
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

                    Xbalance = MathHelper.Clamp(Xbalance, -80, 80);
                    Ybalance = MathHelper.Clamp(Ybalance, 0, 80);
                    blockRender.camOffset.X = -Xbalance / 20;
                    blockRender.camOffset.Y = Ybalance / 20;
                    blockRender.camOffset.X += (float)Math.Sin(shakeTimer / 60 * Math.PI) * shakeTimer / 20;
                    if (shakeTimer > 0)
                        shakeTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    break;
            }
            base.Update(gameTime);
        }
       

        protected override void Draw(GameTime gameTime)                                                                            //Draw
        {

            Rectangle mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (gamestate)
            {
                case GameState.Menu:


                    spriteBatch.Draw(Introscherm,new Vector2((GraphicsDevice.Viewport.Width / 2) - (Introscherm.Width / 2), (GraphicsDevice.Viewport.Height / 4) - (Introscherm.Height / 2)), Color.White);
                    spriteBatch.Draw(StartGame, new Vector2((GraphicsDevice.Viewport.Width / 2) - (StartGame.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (StartGame.Height / 2)), Color.White);
                    spriteBatch.Draw(playbutton, new Vector2((GraphicsDevice.Viewport.Width / 2) - (playbutton.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (playbutton.Height / 2)+ StartGame.Height), Color.White);

                    
                    spriteBatch.DrawString(smallText, "Press M to mute music\n\nPress L to increase Volume\n\nPress K to decrease Volume", new Vector2((GraphicsDevice.Viewport.Width / 5f) , GraphicsDevice.Viewport.Height - 180), Color.White);
                   


                    break;

                case GameState.Game:
                    DrawGame(mainFrame);

                    break;

                case GameState.GameOver:

                    DrawGame(mainFrame);
                    spriteBatch.Draw(Gameoverdim, mainFrame, Color.Black);
                    spriteBatch.DrawString(font, "Game Over", new Vector2((GraphicsDevice.Viewport.Width / 2f) - 90, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                    spriteBatch.DrawString(smallText, "press space to restart \n\n  esc to return to menu", new Vector2((GraphicsDevice.Viewport.Width / 2f) - 200, GraphicsDevice.Viewport.Height / 2 + 70), Color.White);

                    break;

                case GameState.Paused:
                    spriteBatch.Draw(backgr, mainFrame, LevelColors[level]);
                    spriteBatch.DrawString(font, "Paused", new Vector2((GraphicsDevice.Viewport.Width / 2f) - 80, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                    spriteBatch.DrawString(smallText, "press space to continue \n\n  esc to return to menu", new Vector2((GraphicsDevice.Viewport.Width / 2f) - 200, GraphicsDevice.Viewport.Height / 2 + 70), Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawGame(Rectangle mainFrame)
        {
            spriteBatch.Draw(backgr, mainFrame, LevelColors[level]);

            spriteBatch.DrawString(font, "Score: " + Score, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, "Level: " + (level + 1), new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(font2, "Press C \nto hold", new Vector2(60, 110), Color.White);
            spriteBatch.DrawString(font2, "Next", new Vector2(GraphicsDevice.Viewport.Width - 150, 130), Color.White);
            spriteBatch.DrawString(smallText, "Press esc to pauze ", new Vector2((GraphicsDevice.Viewport.Width / 2f) - 170, GraphicsDevice.Viewport.Height - 120), Color.White);

            spriteBatch.End();

            for (int x = 0; x < Playingfield.grid.GetLength(0); x++)
            {
                blockRender.DrawCube(model, x, 0, 0, Color.Gray, 0);
            }


            for (int x = 0; x < Playingfield.grid.GetLength(0); x++)

            {
                for (int y = 1; y < Playingfield.grid.GetLength(1); y++)
                {
                    Cube curCube = Playingfield.GetCube(new GridPos(x, y));
                    if (curCube.cubeType != Cube.CubeType.Empty)
                        blockRender.DrawCube(model, x, y, 0, curCube.color);
                    else if (y <= Playingfield.actualHeight)
                    {
                        blockRender.DrawCube(model, x, y, 0, Color.White, 0.9f);
                    }

                }
            }

            nextBlock.Draw();
            ghostBlock.Draw();
            if (savedBlock != null)
                savedBlock.Draw();
            fallingBlock.Draw();

            spriteBatch.Begin();
        }
    }



   
}
