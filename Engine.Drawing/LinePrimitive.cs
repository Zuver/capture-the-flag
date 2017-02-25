using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Drawing
{
    public class LinePrimitive : AbstractPrimitive
    {
        /// <summary>
        /// Private data
        /// </summary>
        private Vector2 Start;
        private Vector2 End;
        private float Length;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="position"></param>
        /// <param name="endPosition"></param>
        internal LinePrimitive(Color color, Vector2 position, Vector2 endPosition)
            : base(color)
        {
            this.Start = position;
            this.End = endPosition;
            this.Length = (endPosition - position).Length();

            // Build vertices
            this.Vertices = new VertexPositionColor[2];
            this.Vertices[0].Position = new Vector3(this.Start, 0.0f);
            this.Vertices[0].Color = color;
            this.Vertices[1].Position = new Vector3(this.End, 0.0f);
            this.Vertices[1].Color = color;
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public override void SetPosition(Microsoft.Xna.Framework.Vector2 where)
        {
            // Using vector math
            Vector2 startToEnd = this.End - this.Start;
            this.Start = where;
            this.End = where + startToEnd;

            this.Vertices[0].Position = new Vector3(this.Start, 0.0f);
            this.Vertices[1].Position = new Vector3(this.End, 0.0f);

            this.Position = where;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="rotation"></param>
        public override void Rotate(float rotation)
        {
            this.End = new Vector2(this.Length * (float)Math.Cos(rotation), this.Length * (float)Math.Sin(rotation)) + this.Position;
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
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, CameraTransform(this.Vertices), 0, 1);
            }
        }
    }
}