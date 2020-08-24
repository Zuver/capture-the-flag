using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.ProgressBars
{
    public abstract class AbstractProgressBar
    {
        /// <summary>
        /// Primitive used to draw the progress bar
        /// </summary>
        protected AbstractPrimitive Primitive;

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            Primitive.SetPosition(position);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public virtual void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            Primitive.Draw(graphicsDevice, basicEffect);
        }
    }
}
