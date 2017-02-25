using Engine.Camera;
using Engine.Physics.Bodies.Collisions;
using Engine.Physics.Bodies.Collisions.EventHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Physics.Bodies
{
    /// <summary>
    /// This class can't be instantiated because it is declared abstract
    /// </summary>
    public abstract class AbstractBody
    {
        /// <summary>
        /// Position
        /// </summary>
        protected Vector2 Position;

        /// <summary>
        /// Get position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return this.Position;
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractBody SetPosition(Vector2 position)
        {
            this.Position = position;
            return this;
        }

        /// <summary>
        /// Velocity
        /// </summary>
        protected Vector2 Velocity;

        /// <summary>
        /// Get velocity
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVelocity()
        {
            return this.Velocity;
        }

        /// <summary>
        /// Set velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        /// <summary>
        /// Is this body too heavy to move by any force?
        /// </summary>
        private bool Rigid;

        /// <summary>
        /// Acceleration
        /// </summary>
        private Vector2 Acceleration;

        /// <summary>
        /// Mass
        /// </summary>
        private float Mass;

        /// <summary>
        /// Max speed
        /// </summary>
        private float MaxSpeed;

        /// <summary>
        /// Get max speed
        /// </summary>
        /// <returns></returns>
        public float GetMaxSpeed()
        {
            return this.MaxSpeed;
        }

        /// <summary>
        /// Set max speed
        /// </summary>
        /// <param name="maxSpeed"></param>
        public void SetMaxSpeed(float maxSpeed)
        {
            this.MaxSpeed = maxSpeed;
        }

        /// <summary>
        /// Friction coefficient
        /// </summary>
        private float FrictionCoefficient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        public AbstractBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient, bool addToCollisionPool)
        {
            this.Rigid = rigid;
            this.Mass = mass;
            this.MaxSpeed = maxSpeed;
            this.FrictionCoefficient = frictionCoefficient;

            // Put this Body in a state of rest
            this.Acceleration = Vector2.Zero;
            this.Velocity = Vector2.Zero;
            this.Position = Vector2.Zero;

            if (addToCollisionPool)
            {
                // Add this body to the Body collection residing in the CollisionPool class
                CollisionPool.Instance.AddBody(this);
            }
        }

        #region Abstract methods

        public abstract Vector2 GetClosestPointOnPerimeter(Vector2 point);
        protected abstract bool DetectCollision(CircleBody circleBody);
        protected abstract bool DetectCollision(RectangleBody rectangleBody);
        protected abstract bool DetectCollision(LineBody lineBody);

        #endregion

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            if (!this.Rigid)
            {
                this.ApplyFriction();
                this.LimitAcceleration();
                this.LimitVelocity();
                this.UpdatePosition();
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            this.Acceleration = Vector2.Zero;
            this.Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Apply friction
        /// </summary>
        private void ApplyFriction()
        {
            // Apply drag in opposite direction of travel direction
            this.Acceleration *= this.FrictionCoefficient;
            this.Velocity *= this.FrictionCoefficient;
        }

        /// <summary>
        /// Limit acceleration
        /// </summary>
        private void LimitAcceleration()
        {
            // Calculate the current acceleration magnitude
            double magnitude = Math.Sqrt(Math.Pow(this.Acceleration.X, 2.0) + Math.Pow(this.Acceleration.Y, 2.0));

            // If the current magnitude is greater than the limit, then adjust acceleration so that the magnitude is equal to the limit
            if (magnitude > this.MaxSpeed + 0.1f)
            {
                this.Acceleration.Normalize();
                this.Acceleration *= this.MaxSpeed;
            }
        }

        /// <summary>
        /// Limit velocity
        /// </summary>
        private void LimitVelocity()
        {
            // Calculate the current speed
            double speed = Math.Sqrt(Math.Pow(this.Velocity.X, 2.0) + Math.Pow(this.Velocity.Y, 2.0));

            // If the current speed is greater than the speed limit, then adjust velocity so that the speed is equal to the speed limit
            if (speed > this.MaxSpeed + 0.1f)
            {
                this.Velocity.Normalize();
                this.Velocity *= this.MaxSpeed;
            }
        }

        /// <summary>
        /// Update position
        /// </summary>
        private void UpdatePosition()
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity;
        }

        /// <summary>
        /// Apply force
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += force / this.Mass;
        }

        /// <summary>
        /// Detect collision
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool DetectCollision(AbstractBody body)
        {
            bool result = false;

            Type bodyType = body.GetType();

            if (bodyType.Equals(typeof(CircleBody)))
            {
                result = this.DetectCollision((CircleBody)body);
            }
            else if (bodyType.Equals(typeof(RectangleBody)))
            {
                result = this.DetectCollision((RectangleBody)body);
            }
            else if (bodyType.Equals(typeof(LineBody)))
            {
                result = this.DetectCollision((LineBody)body);
            }

            return result;
        }
    }
}