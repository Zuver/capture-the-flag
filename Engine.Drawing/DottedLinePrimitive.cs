using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Drawing
{
    public class DottedLinePrimitive : AbstractPrimitive
    {
        /// <summary>
        /// Start position
        /// </summary>
        private Vector2 _start;

        /// <summary>
        /// End position
        /// </summary>
        private Vector2 _end;

        /// <summary>
        /// Length
        /// </summary>
        private float _length;

        /// <summary>
        /// Line length
        /// </summary>
        private float _lineLength;

        /// <summary>
        /// Line spacing
        /// </summary>
        private float _lineSpacing;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="lineLength"></param>
        /// <param name="lineSpacing"></param>
        internal DottedLinePrimitive(Color color, Vector2 startPosition, Vector2 endPosition, float lineLength, float lineSpacing)
            : base(color)
        {
            this._start = startPosition;
            this._end = endPosition;
            this._length = (endPosition - startPosition).Length();
            this._lineLength = lineLength;
            this._lineSpacing = lineSpacing;

            int numLines = (int)(this._length / (lineLength + lineSpacing));

            this.Vertices = new VertexPositionColor[numLines * 2];

            // Infer directional vector
            Vector2 startToEndUnit = (endPosition - startPosition);
            startToEndUnit.Normalize();

            for (int i = 0; i < this.Vertices.Length; i += 2)
            {
                this.Vertices[i].Position = new Vector3(startPosition, 0.0f);
                this.Vertices[i].Color = color;
                this.Vertices[i + 1].Position = new Vector3(startPosition + (startToEndUnit * lineLength), 0.0f);
                this.Vertices[i + 1].Color = color;
                startPosition += (startToEndUnit * lineLength) + (startToEndUnit * lineSpacing);
            }
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public override void SetPosition(Vector2 where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rotate
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
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, CameraTransform(this.Vertices), 0, this.Vertices.Length / 2);
            }
        }
    }
}
