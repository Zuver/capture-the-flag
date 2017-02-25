using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Physics.Bodies
{
    public class LineBody : AbstractBody
    {
        /// <summary>
        /// End position
        /// </summary>
        private Vector2 EndPosition;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        /// <param name="position"></param>
        /// <param name="endPosition"></param>
        internal LineBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, Vector2 position, Vector2 endPosition)
            : base(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool)
        {
            this.Position = position;
            this.EndPosition = endPosition;
        }

        #region Implementation of abstract methods

        /// <summary>
        /// Get closest point on perimeter of this line
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 GetClosestPointOnPerimeter(Vector2 point)
        {
            point -= this.Position;
            Vector2 startToEnd = this.EndPosition - this.Position;
            float distance = Vector2.Dot(point, startToEnd) / startToEnd.LengthSquared();

            Vector2 result = Vector2.Zero;

            if (distance < 0)    
            {
                result = this.Position;

            }
            else if (distance > 1)
            {
                result = this.EndPosition;
            }
            else
            {
                result = startToEnd * distance + this.Position;
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