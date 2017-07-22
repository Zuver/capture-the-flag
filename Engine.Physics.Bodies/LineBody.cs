using Microsoft.Xna.Framework;

namespace Engine.Physics.Bodies
{
    public class LineBody : AbstractBody
    {
        /// <summary>
        /// End position
        /// </summary>
        private readonly Vector2 _endPosition;

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
            Position = position;
            _endPosition = endPosition;
        }

        #region Implementation of abstract methods

        /// <summary>
        /// Get closest point on perimeter of this line
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 GetClosestPointOnPerimeter(Vector2 point)
        {
            point -= Position;
            Vector2 startToEnd = _endPosition - Position;
            float distance = Vector2.Dot(point, startToEnd) / startToEnd.LengthSquared();

            Vector2 result;

            if (distance < 0)    
            {
                result = Position;

            }
            else if (distance > 1)
            {
                result = _endPosition;
            }
            else
            {
                result = startToEnd * distance + Position;
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