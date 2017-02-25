using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Physics.Bodies
{
    public static class BodyFactory
    {
        /// <summary>
        /// Constructs a circle body
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static CircleBody Circle(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, float radius)
        {
            return new CircleBody(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool, radius);
        }

        /// <summary>
        /// Constructs a rectangle body
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static RectangleBody Rectangle(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, Vector2 size)
        {
            return new RectangleBody(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool, size);
        }

        /// <summary>
        /// Constructs a line body
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        /// <param name="position"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public static LineBody Line(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool, Vector2 position, Vector2 endPosition)
        {
            return new LineBody(rigid, mass, maxSpeed, frictionCoefficient, addToCollisionPool, position, endPosition);
        }
    }
}