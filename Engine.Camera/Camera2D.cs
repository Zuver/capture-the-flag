using Microsoft.Xna.Framework;

namespace Engine.Camera
{
    public sealed class Camera2D
    {
        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static Camera2D _instance;

        public static Camera2D Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Camera2D();
                }

                return _instance;
            }
        }

        private Vector2 _targetPosition;
        private Vector2 _position;

        private Vector2 _velocity;
        private float _maxPanSpeed;

        private Vector2 _acceleration;
        private float _maxPanAcceleration;

        /// <summary>
        /// Constructor
        /// </summary>
        private Camera2D()
        {
            // Put this camera in a state of rest
            _targetPosition = Vector2.Zero;
            _position = Vector2.Zero;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxPanSpeed"></param>
        /// <param name="maxPanAcceleration"></param>
        public void Initialize(Vector2 position, float maxPanSpeed, float maxPanAcceleration)
        {
            _position = position;
            _maxPanSpeed = maxPanSpeed;
            _maxPanAcceleration = maxPanAcceleration;
        }

        /// <summary>
        /// Set target position
        /// </summary>
        /// <param name="targetPosition"></param>
        public void SetTargetPosition(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        /// <summary>
        /// Get position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            float epsilon = 5f;

            if (Vector2.DistanceSquared(_position, _targetPosition) > epsilon * epsilon)
            {
                _acceleration += _targetPosition - _position;

                LimitAcceleration();
                _velocity += _acceleration;

                LimitVelocity();
                _position += _velocity;
            }
            else
            {
                _acceleration = Vector2.Zero;
                _velocity = Vector2.Zero;
            }
        }

        /// <summary>
        /// Stop the camera from moving
        /// </summary>
        public void Freeze()
        {
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Limit velocity
        /// </summary>
        private void LimitVelocity()
        {
            if (_velocity.LengthSquared() > _maxPanSpeed * _maxPanSpeed)
            {
                _velocity.Normalize();
                _velocity *= _maxPanSpeed;
            }

            Vector2 toTarget = _targetPosition - _position;

            if (float.IsNaN(toTarget.X))
                toTarget.X = 0f;
            if (float.IsNaN(toTarget.Y))
                toTarget.Y = 0f;

            _velocity *= toTarget.Length() / 50f;
        }

        /// <summary>
        /// Limit acceleration
        /// </summary>
        private void LimitAcceleration()
        {
            if (_acceleration.LengthSquared() > _maxPanAcceleration * _maxPanAcceleration)
            {
                _acceleration.Normalize();
                _acceleration *= _maxPanAcceleration;
            }
        }
    }
}
