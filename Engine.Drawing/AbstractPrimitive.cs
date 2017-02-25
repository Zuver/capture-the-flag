using Engine.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Drawing
{
    public abstract class AbstractPrimitive
    {
        /// <summary>
        /// Is visible?
        /// </summary>
        protected bool IsVisible;

        /// <summary>
        /// Protected data
        /// </summary>
        protected VertexPositionColor[] Vertices;

        /// <summary>
        /// Position
        /// </summary>
        protected Vector2 Position;

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public abstract void SetPosition(Vector2 where);

        /// <summary>
        /// Color
        /// </summary>
        protected Color Color;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        internal AbstractPrimitive(Color color)
        {
            this.IsVisible = true;
            this.Color = color;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="rotation">Rotation</param>
        public abstract void Rotate(float rotation);
        
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public abstract void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect);

        /// <summary>
        /// Camera transform
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static VertexPositionColor[] CameraTransform(VertexPositionColor[] source)
        {
            VertexPositionColor[] result = new VertexPositionColor[source.Length];
            Vector2 cameraPosition = Camera2D.Instance.GetPosition();
            source.CopyTo(result, 0);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Position.X -= cameraPosition.X;
                result[i].Position.Y -= cameraPosition.Y;
            }

            return result;
        }
    }
}