using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        //Camera
        Vector3 camTarget, camPosition, positionModel, colorVect;
        Matrix projectionMatrix, viewMatrix, worldMatrix;
        Model model, model2;
        
        enum gameState { Menu, Game, End };
        gameState curGamestate = gameState.Menu;
        enum blockState { Tblock, Lblock, RLblock, Iblock, Sblock, Zblock, Cube };
        blockState curBlockstate = blockState.RLblock;
    
    public Game1()
        {
            

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {


            base.Initialize();
        }


        protected override void LoadContent()
        {

            //Setup Camera
            
            camTarget = new Vector3(0f, 1f, 0f);
          
            colorVect = new Vector3(0, 0, 0);

            camPosition = new Vector3(0f, -1, -10f);
            positionModel = new Vector3(0f, 0f, 1f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, positionModel, Vector3.Up);
            model = Content.Load<Model>("MonoCube");
            model2 = Content.Load<Model>("MonoCube");

        }
        void Reset()
        {
            
        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {



            if (curGamestate == gameState.Menu)                                             //Menu
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    curGamestate = gameState.Game;
                }

            }
                if (curGamestate == gameState.Game)                                             //Game

                {
                    camPosition.Y += 0.1f;
                    camTarget.Y += 0.1f;
                    if (camTarget.Y >= 0 && camPosition.Y >= 1)
                    {
                        camPosition.Y -= 0.1f;
                        camTarget.Y -= 0.1f;
                    }


                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        camPosition.X -= 0.1f;
                        camTarget.X -= 0.1f;

                        if (camTarget.X <= -4.5f && camPosition.X <= -4.5f)
                        {
                            camPosition.X += 0.1f;
                            camTarget.X += 0.1f;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        camPosition.X += 0.1f;
                        camTarget.X += 0.1f;
                        if (camTarget.X >= 4.5 && camPosition.X >= 4.5)
                        {
                            camPosition.X -= 0.1f;
                            camTarget.X -= 0.1f;
                        }

                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        camPosition.Y += 0.1f;
                        camTarget.Y += 0.1f;

                        if (camTarget.Y >= 0 && camPosition.Y >= 1)
                        {
                            camPosition.Y -= 0.1f;
                            camTarget.Y -= 0.1f;
                        }

                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Q))                                  // Kleur verandering bij indrukken van Q toets
                    {
                        colorVect.X = 20;
                        colorVect.Y = 0;
                        colorVect.Z = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.T))                                  // Kleur verandering bij indrukken van T toets
                    {
                        colorVect.X = 0;
                        colorVect.Y = 0;
                        colorVect.Z = 20;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.W))                                  // Kleur verandering bij indrukken van W toets
                    {
                        colorVect.X = 0;
                        colorVect.Y = 20;
                        colorVect.Z = 20;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.E))                                  // Kleur verandering bij indrukken van E toets
                    {
                        colorVect.X = 20;
                        colorVect.Y = 0;
                        colorVect.Z = 20;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R))                                  // Kleur verandering bij indrukken van R toets
                    {
                        colorVect.X = 20;
                        colorVect.Y = 20;
                        colorVect.Z = 0;
                    }
                    viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

                
                

            }base.Update(gameTime);

        }

    
        

        protected override void Draw(GameTime gameTime)
        {
            if (curGamestate == gameState.Menu)                                             //Menu
            {

                GraphicsDevice.Clear(Color.White);

            }

            if (curGamestate == gameState.Game)                                             //Game

            {
                GraphicsDevice.Clear(Color.White);
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {

                        effect.EnableDefaultLighting();
                        effect.AmbientLightColor = colorVect;
                        effect.View = viewMatrix;
                        effect.World = worldMatrix;
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();

                }
                base.Draw(gameTime);
            }
        }
    }
}
