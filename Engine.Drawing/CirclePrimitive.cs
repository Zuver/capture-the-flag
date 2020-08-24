using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class CirclePrimitive : AbstractPrimitive
    {
        /// <summary>
        /// Private data
        /// </summary>
        private readonly float _radius;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="radius"></param>
        /// <param name="numVertices"></param>
        internal CirclePrimitive(Color color, float radius, int numVertices)
            : base(color)
        {
            _radius = radius;

            // Build vertices
            Vertices = new VertexPositionColor[numVertices * 2 + 2];
            SetVertices(0.0f);
            SetColor(color);
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public override void SetPosition(Vector2 where)
        {
            // Update all vertices by using the previous position
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Position += new Vector3(where - Position, 0.0f);
            }

            Position = where;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="rotation"></param>
        public override void Rotate(float rotation)
        {
            SetVertices(rotation);
        }

        public override void SetFillPercentage(float fillPercentage)
        {
            FillPercentage = fillPercentage;
            SetColor(Color);
        }

        /// <summary>
        /// Set vertices
        /// </summary>
        /// <param name="rotation"></param>
        private void SetVertices(float rotation)
        {
            // First and last vertex will tie the circle together
            Vertices[0].Position = _radius * new Vector3((float)Math.Cos(rotation), (float)Math.Sin(rotation), 0.0f) +
                                   new Vector3(Position, 0.0f);
            Vertices[Vertices.Length - 1].Position = _radius *
                                                     new Vector3((float)Math.Cos(rotation), (float)Math.Sin(rotation),
                                                         0.0f) + new Vector3(Position, 0.0f);

            // Middle vertices
            for (int i = 1; i < (Vertices.Length - 2) / 2 + 1; i++)
            {
                double theta = MathHelper.TwoPi * (i / ((float)(Vertices.Length - 2) / 2)) + rotation;
                Vector3 position = _radius * new Vector3((float)Math.Cos(theta), (float)Math.Sin(theta), 0.0f);

                Vertices[i * 2 - 1].Position = position + new Vector3(Position, 0.0f);
                Vertices[i * 2].Position = position + new Vector3(Position, 0.0f);
            }
        }

        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color)
        {
            for (int i = 0; i < Vertices.Length * FillPercentage; i++)
            {
                Vertices[i].Color = color;
            }

            for (int i = (int)(Vertices.Length * FillPercentage); i < Vertices.Length; i++)
            {
                Vertices[i].Color = new Color(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            if (IsVisible)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, CameraTransform(Vertices), 0,
                    Vertices.Length / 2);
            }
        }
    }
}
