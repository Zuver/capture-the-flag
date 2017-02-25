using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine.Camera
{
    public sealed class Camera2D
    {
        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static Camera2D instance;
        public static Camera2D Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera2D();
                }

                return instance;
            }
        }

        private Vector2 TargetPosition;
        private Vector2 Position;

        private Vector2 Velocity;
        private float MaxPanSpeed;

        private Vector2 Acceleration;
        private float MaxPanAcceleration;

        /// <summary>
        /// Constructor
        /// </summary>
        private Camera2D()
        {
            // Put this camera in a state of rest
            this.TargetPosition = Vector2.Zero;
            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxPanSpeed"></param>
        /// <param name="maxPanAcceleration"></param>
        public void Initialize(Vector2 position, float maxPanSpeed, float maxPanAcceleration)
        {
            this.Position = position;
            this.MaxPanSpeed = maxPanSpeed;
            this.MaxPanAcceleration = maxPanAcceleration;
        }

        /// <summary>
        /// Set target position
        /// </summary>
        /// <param name="targetPosition"></param>
        public void SetTargetPosition(Vector2 targetPosition)
        {
            this.TargetPosition = targetPosition;
        }

        /// <summary>
        /// Get position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return this.Position;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            float epsilon = 5f;

            if (Vector2.DistanceSquared(this.Position, this.TargetPosition) > epsilon * epsilon)
            {
                this.Acceleration += this.TargetPosition - this.Position;

                this.LimitAcceleration();
                this.Velocity += this.Acceleration;

                this.LimitVelocity();
                this.Position += this.Velocity;
            }
            else
            {
                this.Acceleration = Vector2.Zero;
                this.Velocity = Vector2.Zero;
            }
        }

        /// <summary>
        /// Stop the camera from moving
        /// </summary>
        public void Freeze()
        {
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Limit velocity
        /// </summary>
        private void LimitVelocity()
        {
            if (this.Velocity.LengthSquared() > this.MaxPanSpeed * this.MaxPanSpeed)
            {
                this.Velocity.Normalize();
                this.Velocity *= this.MaxPanSpeed;
            }

            Vector2 toTarget = this.TargetPosition - this.Position;

            if (float.IsNaN(toTarget.X))
                toTarget.X = 0f;
            if (float.IsNaN(toTarget.Y))
                toTarget.Y = 0f;

            this.Velocity *= toTarget.Length() / 50f;
        }

        /// <summary>
        /// Limit acceleration
        /// </summary>
        private void LimitAcceleration()
        {
            if (this.Acceleration.LengthSquared() > this.MaxPanAcceleration * this.MaxPanAcceleration)
            {
                this.Acceleration.Normalize();
                this.Acceleration *= this.MaxPanAcceleration;
            }
        }
    }
}