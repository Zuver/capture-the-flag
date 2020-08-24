using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Drawing
{
    public class RectanglePrimitive : AbstractPrimitive
    {
        /// <summary>
        /// Private data
        /// </summary>
        private int Width;

        private int Height;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        internal RectanglePrimitive(int width, int height, Color color)
            : base(color)
        {
            this.Width = width;
            this.Height = height;

            // Build vertices
            this.Vertices = new VertexPositionColor[5];
            this.Vertices[0].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, -this.Height / 2), 0.0f);
            this.Vertices[0].Color = color;
            this.Vertices[1].Position = new Vector3(this.Position + new Vector2(this.Width / 2, -this.Height / 2), 0.0f);
            this.Vertices[1].Color = color;
            this.Vertices[2].Position = new Vector3(this.Position + new Vector2(this.Width / 2, this.Height / 2), 0.0f);
            this.Vertices[2].Color = color;
            this.Vertices[3].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, this.Height / 2), 0.0f);
            this.Vertices[3].Color = color;
            this.Vertices[4].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, -this.Height / 2), 0.0f);
            this.Vertices[4].Color = color;
        }

        /// <summary>
        /// Set the position of the top-left corner
        /// </summary>
        /// <param name="where">Position of the top-left corner</param>
        public override void SetPosition(Vector2 where)
        {
            this.Position = where;

            this.Vertices[0].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, -this.Height / 2), 0.0f);
            this.Vertices[1].Position = new Vector3(this.Position + new Vector2(this.Width / 2, -this.Height / 2), 0.0f);
            this.Vertices[2].Position = new Vector3(this.Position + new Vector2(this.Width / 2, this.Height / 2), 0.0f);
            this.Vertices[3].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, this.Height / 2), 0.0f);
            this.Vertices[4].Position = new Vector3(this.Position + new Vector2(-this.Width / 2, -this.Height / 2), 0.0f);
        }

        /// <summary>
        /// Rotate (not implemented)
        /// </summary>
        /// <param name="rotation"></param>
        public override void Rotate(float rotation)
        {
            throw new NotImplementedException();
        }

        public override void SetFillPercentage(float fillPercentage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            if (this.IsVisible)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, CameraTransform(this.Vertices), 0, 4);
            }
        }
    }
}
