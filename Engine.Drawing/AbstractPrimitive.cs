using Engine.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        /// Fill percentage
        /// </summary>
        public float FillPercentage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        internal AbstractPrimitive(Color color)
        {
            FillPercentage = 1.0f;
            IsVisible = true;
            Color = color;
        }

        /// <summary>
        /// Set fill percentage
        /// </summary>
        /// <param name="fillPercentage"></param>
        public abstract void SetFillPercentage(float fillPercentage);

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
