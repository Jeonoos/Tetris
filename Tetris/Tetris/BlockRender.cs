﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{

    public class BlockRender : Game
    {
        GraphicsDeviceManager graphics;
       
        public Vector3 camTarget;
        public Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        bool orbit = false;
        public Vector2 camOffset = Vector2.Zero;
        public BlockRender(GraphicsDeviceManager graphics) {
            this.graphics = graphics;
            SetupCamera();
        }

        protected override void Initialize()
        {


            base.Initialize();
        }


        public void SetupCamera()
        {

            //Setup Camera
            camTarget = new Vector3(10f, 20f, 0f);
            camPosition = new Vector3(10f, 20f, 70f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            

        }

      
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X -= 0.1f;
                camTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                    orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));

                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);


            base.Update(gameTime);
        }

        public void DrawCube(Model model, int x, int y, Color color, float transparancy = 0)
        {
            viewMatrix = Matrix.CreateLookAt(camPosition + Vector3.Right * camOffset.X + Vector3.Up * camOffset.Y, camTarget, new Vector3(0f, 1f, 0f));
            Vector3 modelPosition = new Vector3(x * 2f,y * 2f, 0);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3((float)color.R/255,(float)color.G/255,(float)color.B/255);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateTranslation(modelPosition);
                    effect.Projection = projectionMatrix;
                    graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    effect.Alpha = 1 - transparancy;
                }
                mesh.Draw();

            }
        }
    }
}
