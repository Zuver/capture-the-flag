using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Physics.Bodies
{
    public class RectangleBody : AbstractBody
    {
        /// <summary>
        /// Size
        /// </summary>
        private Vector2 Size;

        /// <summary>
        /// Get size
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize()
        {
            return this.Size;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="size"></param>
        internal RectangleBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, Vector2 size)
            : base(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool)
        {
            this.Size = size;
        }

        #region Implementation of abstract methods

        /// <summary>
        /// Get closest point on perimeter of this rectangle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 GetClosestPointOnPerimeter(Vector2 point)
        {
            Vector2 result = point;
            Vector2 rectangleHalfSize = this.Size / 2;

            if (result.X < this.Position.X - rectangleHalfSize.X)
            {
                result.X = this.Position.X - rectangleHalfSize.X;
            }
            else if (result.X > this.Position.X + rectangleHalfSize.X)
            {
                result.X = this.Position.X + rectangleHalfSize.X;
            }

            if (result.Y < this.Position.Y - rectangleHalfSize.Y)
            {
                result.Y = this.Position.Y - rectangleHalfSize.Y;
            }
            else if (result.Y > this.Position.Y + rectangleHalfSize.Y)
            {
                result.Y = this.Position.Y + rectangleHalfSize.Y;
            }

            return result;
        }

        /// <summary>
        /// Detect collision with a circle body
        /// </summary>
        /// <param name="circleBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(CircleBody circleBody)
        {
            return circleBody.DetectCollision(this);
        }

        /// <summary>
        /// Detect collision with a rectangle body
        /// </summary>
        /// <param name="rectangleBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(RectangleBody rectangleBody)
        {
            return false;
        }

        /// <summary>
        /// Detect collision with a line body
        /// </summary>
        /// <param name="lineBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(LineBody lineBody)
        {
            return false;
        }

        #endregion
    }
}