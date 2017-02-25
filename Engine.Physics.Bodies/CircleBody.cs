using Engine.Physics.Bodies.Collisions.EventHandlers;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Physics.Bodies
{
    public class CircleBody : AbstractBody
    {
        /// <summary>
        /// Radius
        /// </summary>
        private float Radius;

        /// <summary>
        /// Get radius
        /// </summary>
        /// <returns></returns>
        public float GetRadius()
        {
            return this.Radius;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="radius"></param>
        internal CircleBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, float radius)
            : base(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool)
        {
            this.Radius = radius;
        }

        #region Implementation of abstract methods

        /// <summary>
        /// Get closest point on perimeter of this circle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 GetClosestPointOnPerimeter(Vector2 point)
        {
            Vector2 toPointUnit = point - this.Position;
            toPointUnit.Normalize();

            return this.Position + this.Radius * toPointUnit;
        }

        /// <summary>
        /// Detect a collision with a circle body
        /// </summary>
        /// <param name="circleBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(CircleBody circleBody)
        {
            // Get sum of radii
            float radiusSum = this.Radius + circleBody.Radius;

            // Get distance between the two bodies
            Vector2 delta = this.Position - circleBody.Position;

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
            return (rectangleBody.GetClosestPointOnPerimeter(this.Position) - this.Position).LengthSquared() < this.Radius * this.Radius;
        }

        /// <summary>
        /// Detect a collision with a line body
        /// </summary>
        /// <param name="lineBody"></param>
        /// <returns></returns>
        protected override bool DetectCollision(LineBody lineBody)
        {
            Vector2 closestPointOnLineBody = lineBody.GetClosestPointOnPerimeter(this.Position);

            bool touchingLine = ((closestPointOnLineBody - this.Position).LengthSquared() < this.Radius * this.Radius);
            bool willCrossLine = !(this.Position - closestPointOnLineBody).HasSameDirection(this.Position - this.Velocity - lineBody.GetClosestPointOnPerimeter(this.Position - this.Velocity));
            bool result = touchingLine || willCrossLine;

            return result;
        }

        #endregion
    }
}