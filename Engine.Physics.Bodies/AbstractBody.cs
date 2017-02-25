using System;
using Engine.Physics.Bodies.Collisions;
using Microsoft.Xna.Framework;

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
            return Position;
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractBody SetPosition(Vector2 position)
        {
            Position = position;
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
            return Velocity;
        }

        /// <summary>
        /// Set velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// Is this body too heavy to move by any force?
        /// </summary>
        private readonly bool _rigid;

        /// <summary>
        /// Acceleration
        /// </summary>
        private Vector2 _acceleration;

        /// <summary>
        /// Mass
        /// </summary>
        private readonly float _mass;

        /// <summary>
        /// Max speed
        /// </summary>
        private float _maxSpeed;

        /// <summary>
        /// Get max speed
        /// </summary>
        /// <returns></returns>
        public float GetMaxSpeed()
        {
            return _maxSpeed;
        }

        /// <summary>
        /// Set max speed
        /// </summary>
        /// <param name="maxSpeed"></param>
        public void SetMaxSpeed(float maxSpeed)
        {
            _maxSpeed = maxSpeed;
        }

        /// <summary>
        /// Friction coefficient
        /// </summary>
        private readonly float _frictionCoefficient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rigid"></param>
        /// <param name="mass"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="frictionCoefficient"></param>
        /// <param name="addToCollisionPool"></param>
        protected AbstractBody(bool rigid, float mass, float maxSpeed, float frictionCoefficient,
            bool addToCollisionPool)
        {
            _rigid = rigid;
            _mass = mass;
            _maxSpeed = maxSpeed;
            _frictionCoefficient = frictionCoefficient;

            // Put this Body in a state of rest
            _acceleration = Vector2.Zero;
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;

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
            if (!_rigid)
            {
                ApplyFriction();
                LimitAcceleration();
                LimitVelocity();
                UpdatePosition();
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            _acceleration = Vector2.Zero;
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Apply friction
        /// </summary>
        private void ApplyFriction()
        {
            // Apply drag in opposite direction of travel direction
            _acceleration *= _frictionCoefficient;
            Velocity *= _frictionCoefficient;
        }

        /// <summary>
        /// Limit acceleration
        /// </summary>
        private void LimitAcceleration()
        {
            // Calculate the current acceleration magnitude
            double magnitude = Math.Sqrt(Math.Pow(_acceleration.X, 2.0) + Math.Pow(_acceleration.Y, 2.0));

            // If the current magnitude is greater than the limit, then adjust acceleration so that the magnitude is equal to the limit
            if (magnitude > _maxSpeed + 0.1f)
            {
                _acceleration.Normalize();
                _acceleration *= _maxSpeed;
            }
        }

        /// <summary>
        /// Limit velocity
        /// </summary>
        private void LimitVelocity()
        {
            // Calculate the current speed
            double speed = Math.Sqrt(Math.Pow(Velocity.X, 2.0) + Math.Pow(Velocity.Y, 2.0));

            // If the current speed is greater than the speed limit, then adjust velocity so that the speed is equal to the speed limit
            if (speed > _maxSpeed + 0.1f)
            {
                Velocity.Normalize();
                Velocity *= _maxSpeed;
            }
        }

        /// <summary>
        /// Update position
        /// </summary>
        private void UpdatePosition()
        {
            Velocity += _acceleration;
            Position += Velocity;
        }

        /// <summary>
        /// Apply force
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector2 force)
        {
            _acceleration += force / _mass;
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

            if (bodyType == typeof(CircleBody))
            {
                result = DetectCollision((CircleBody) body);
            }
            else if (bodyType == typeof(RectangleBody))
            {
                result = DetectCollision((RectangleBody) body);
            }
            else if (bodyType == typeof(LineBody))
            {
                result = DetectCollision((LineBody) body);
            }

            return result;
        }
    }
}
