using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Drawing
{
    public sealed class PrimitiveBuilder : AbstractPrimitive
    {
        /// <summary>
        /// Primitive list
        /// </summary>
        private List<AbstractPrimitive> primitiveList;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        public PrimitiveBuilder(Color color)
            : base(color)
        {
            this.primitiveList = new List<AbstractPrimitive>();
        }

        /// <summary>
        /// Add a primitive to the collection
        /// </summary>
        /// <param name="primitive"></param>
        public void Add(AbstractPrimitive primitive)
        {
            // Add to list
            this.primitiveList.Add(primitive);

            // Update the position of the newly-added primitive
            primitive.SetPosition(this.Position);
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="where"></param>
        public override void SetPosition(Microsoft.Xna.Framework.Vector2 where)
        {
            foreach (AbstractPrimitive item in this.primitiveList)
            {
                item.SetPosition(where);
            }

            this.Position = where;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="rotation"></param>
        public override void Rotate(float rotation)
        {
            foreach (AbstractPrimitive item in primitiveList)
            {
                item.Rotate(rotation);
            }
        }

        public override void SetFillPercentage(float fillPercentage)
        {
            foreach (AbstractPrimitive item in primitiveList)
            {
                item.SetFillPercentage(fillPercentage);
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice,
            Microsoft.Xna.Framework.Graphics.BasicEffect basicEffect)
        {
            foreach (AbstractPrimitive item in primitiveList)
            {
                item.Draw(graphicsDevice, basicEffect);
            }
        }
    }
}
