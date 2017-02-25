using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Drawing
{
    public class CirclePrimitive : AbstractPrimitive
    {
        /// <summary>
        /// Private data
        /// </summary>
        private float Radius;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="radius"></param>
        /// <param name="numVertices"></param>
        internal CirclePrimitive(Color color, float radius, int numVertices)
            : base(color)
        {
            this.Radius = radius;

            // Build vertices
            this.Vertices = new VertexPositionColor[numVertices * 2 + 2];
            this.SetVertices(0.0f);
            this.SetColor(color);
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public override void SetPosition(Vector2 where)
        {
            // Update all vertices by using the previous position
            for (int i = 0; i < this.Vertices.Length; i++)
            {
                this.Vertices[i].Position += new Vector3(where - this.Position, 0.0f);
            }

            this.Position = where;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="rotation"></param>
        public override void Rotate(float rotation)
        {
            this.SetVertices(rotation);
        }

        /// <summary>
        /// Set vertices
        /// </summary>
        /// <param name="rotation"></param>
        private void SetVertices(float rotation)
        {
            // First and last vertex will tie the circle together
            this.Vertices[0].Position = this.Radius * new Vector3((float)Math.Cos(rotation), (float)Math.Sin(rotation), 0.0f) + new Vector3(this.Position, 0.0f);
            this.Vertices[this.Vertices.Length - 1].Position = this.Radius * new Vector3((float)Math.Cos(rotation), (float)Math.Sin(rotation), 0.0f) + new Vector3(this.Position, 0.0f); ;

            // Middle vertices
            for (int i = 1; i < (this.Vertices.Length - 2) / 2 + 1; i++)
            {
                double theta = MathHelper.TwoPi * ((float)i / ((float)(this.Vertices.Length - 2) / 2)) + rotation;
                Vector3 position = this.Radius * new Vector3((float)Math.Cos(theta), (float)Math.Sin(theta), 0.0f);

                this.Vertices[i * 2 - 1].Position = position + new Vector3(this.Position, 0.0f); ;
                this.Vertices[i * 2].Position = position + new Vector3(this.Position, 0.0f); ;
            }
        }

        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color)
        {
            for (int i = 0; i < this.Vertices.Length; i++)
            {
                this.Vertices[i].Color = color;
            }
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
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, CameraTransform(this.Vertices), 0, this.Vertices.Length / 2);
            }
        }
    }
}