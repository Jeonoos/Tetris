using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{

    public class BlockRender : Game
    {
        GraphicsDeviceManager graphics;
        public Vector3 camTarget,camPosition;
        Matrix projectionMatrix,viewMatrix,worldMatrix;
        bool orbit = false;
        public Vector2 camOffset = Vector2.Zero;
        public BlockRender(GraphicsDeviceManager graphics) {
            this.graphics = graphics;
            SetupCamera();
        }


        public void SetupCamera()                                       //Camera
        {

            
            camTarget = new Vector3(10f, 20f, 0f);
            camPosition = new Vector3(10f, 20f, 70f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            


        }

        public void DrawCube(Model model, int x, int y, int z, Color color, float transparancy = 0)             //Cube drawing
        {
            viewMatrix = Matrix.CreateLookAt(camPosition + Vector3.Right * camOffset.X + Vector3.Up * camOffset.Y, camTarget, new Vector3(0f, 1f, 0f));
            Vector3 modelPosition = new Vector3(x * 2f,y * 2f, z);
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
