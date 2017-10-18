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
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        Texture2D monotex;
        PreviewTetrisBlock nextBlock;
        TetrisBlock fallingBlock;
        public static Model model;
        public static Model model2;
        public float falltimer = 0;
        public float inputtimer = 0;
        public float shakeTimer;
        public static GameState gamestate = GameState.Game;
        public enum GameState { Game, GameOver}
        public static Game1 game;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 720;
            //graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            Playingfield.graphics = graphics;
        }

        protected override void Initialize() {
            Playingfield.Initialize(this);

            base.Initialize();
        }
  
        protected override void LoadContent() {
            Playingfield.LoadContent(this);
            //backgr = Content.Load <Texture2D>("background");
            //model = Content.Load<Model>("monocube");
            //monotex = Content.Load<Texture2D>("monotex");
            //TetrisSong = Content.Load<Song>("Tetris");
            //MediaPlayer.Play(TetrisSong);
            //MediaPlayer.IsRepeating = true;
        }

      
        protected override void UnloadContent() {
            
        }

        KeyboardState kstate = Keyboard.GetState();
        KeyboardState oldkstate;
        float Xbalance = 0;
        float Ybalance = 0;
        bool BreakFalling = false;
        double gameTimer = 0;
        float gameOverRotate = 0;
        protected override void Update(GameTime gameTime) {

            if (kstate.IsKeyDown(Keys.Escape))
                Exit();

            falltimer += gameTime.ElapsedGameTime.Milliseconds;
            gameTimer += gameTime.ElapsedGameTime.Milliseconds;
            oldkstate = kstate;
            kstate = Keyboard.GetState();


            TetrisBlock fallingBlock = Playingfield.fallingBlock;
            PreviewTetrisBlock nextBlock = Playingfield.nextBlock;
            if (gamestate == GameState.GameOver)
            {
                gameOverRotate += gameTime.ElapsedGameTime.Milliseconds * 0.5f;
                Playingfield.blockRender.camOffset = Vector2.Zero;
                Playingfield.blockRender.camPosition += (new Vector3((float)Math.Sin(gameOverRotate / 1000) * 75 + Playingfield.blockRender.camTarget.X, Playingfield.blockRender.camPosition.Y, (float)Math.Cos(gameOverRotate / 1000) * 75 + Playingfield.blockRender.camTarget.Z) -Playingfield.blockRender.camPosition) * 0.1f;
            }
            else
            {
                if (falltimer > 500)
                {
                    if (fallingBlock.CheckCollision(new GridPos(fallingBlock.pos.x, fallingBlock.pos.y - 1)))
                    {
                        fallingBlock.Solidify();
                        Playingfield.CheckForRow();
                        if (kstate.IsKeyDown(Keys.Down))
                        {
                            shakeTimer = 300;
                            BreakFalling = true;
                        }
                        fallingBlock = new TetrisBlock(nextBlock.type);
                        nextBlock = new PreviewTetrisBlock(Playingfield.random.Next(0, 7));
                    }
                    else
                    {
                        fallingBlock.pos.y -= 1;
                        falltimer = 0;
                    }
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
                else if (oldkstate.IsKeyDown(Keys.Right)){
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
                    falltimer = 500;
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
                Playingfield.blockRender.camOffset.X = -Xbalance / 20;
                Playingfield.blockRender.camOffset.Y = Ybalance / 20;
                Playingfield.blockRender.camOffset.X += (float)Math.Sin(shakeTimer / 60 * Math.PI) * shakeTimer / 20;
                if (shakeTimer > 0)
                    shakeTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            base.Update(gameTime);
        }

        public void Gameover() {
            gamestate = GameState.GameOver;
            gameTimer = 0;
        }

        protected override void Draw(GameTime gameTime) {
            Playingfield.Draw(gameTime, this);
            base.Draw(gameTime);

        }
    }



   
}
