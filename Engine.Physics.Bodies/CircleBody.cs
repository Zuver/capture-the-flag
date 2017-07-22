using Engine.Utilities;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Bodies
{
    public class CircleBody : AbstractBody
    {
        /// <summary>
        /// Radius
        /// </summary>
        private readonly float _radius;

        /// <summary>
        /// Get radius
        /// </summary>
        /// <returns></returns>
        public float GetRadius()
        {
            return _radius;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        /// <param name="radius"></param>
        internal CircleBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, float radius)
            : base(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool)
        {
            _radius = radius;
        }

        #region Implementation of abstract methods

        /// <summary>
        /// Get closest point on perimeter of this circle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 GetClosestPointOnPerimeter(Vector2 point)
        {
            Vector2 toPointUnit = point - Position;
            toPointUnit.Normalize();

            return Position + _radius * toPointUnit;
        }

        /// <summary>
        /// Detect a collision with a circle body
        /// </summary>
        /// <param name="circleBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(CircleBody circleBody)
        {
            // Get sum of radii
            float radiusSum = _radius + circleBody._radius;

            // Get distance between the two bodies
            Vector2 delta = Position - circleBody.Position;

            // Return true if the distance between the center of the two bodies
            // is less than or equal to the sum of the radii
            bool result = (radiusSum * radiusSum) >= (delta.X * delta.X + delta.Y * delta.Y);

            return result;
        }

        /// <summary>
        /// Detect a collision with a rectangle body
        /// </summary>
        /// <param name="rectangleBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(RectangleBody rectangleBody)
        {
            return (rectangleBody.GetClosestPointOnPerimeter(Position) - Position).LengthSquared() < _radius * _radius;
        }

        /// <summary>
        /// Detect a collision with a line body
        /// </summary>
        /// <param name="lineBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(LineBody lineBody)
        {
            Vector2 closestPointOnLineBody = lineBody.GetClosestPointOnPerimeter(Position);

            // Is the line "inside" the circle?
            bool isTouchingLine = ((closestPointOnLineBody - Position).LengthSquared() < _radius * _radius);
            bool willCrossLine = !(Position - closestPointOnLineBody).HasSameDirection(Position - Velocity - lineBody.GetClosestPointOnPerimeter(Position - Velocity));

            return isTouchingLine || willCrossLine;
        }

        #endregion
    }
}