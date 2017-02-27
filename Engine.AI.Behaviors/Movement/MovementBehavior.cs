using System.Collections.Generic;
using System.Linq;
using Engine.Drawing;
using Engine.Entities.Graphs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.AI.Behaviors.Movement
{
    public class MovementBehavior
    {
        /// <summary>
        /// Goal
        /// </summary>
        private Vector2 _goal;

        /// <summary>
        /// Get goal
        /// </summary>
        /// <returns></returns>
        public Vector2 GetGoal()
        {
            return _goal;
        }

        /// <summary>
        /// Set goal
        /// </summary>
        /// <param name="goalPosition"></param>
        /// <param name="routeToGoal"></param>
        public void SetGoal(Vector2 goalPosition, List<Node> routeToGoal)
        {
            _goal = goalPosition;

            foreach (Node node in routeToGoal)
            {
                EnqueueWaypoint(node.Position);
            }

            EnqueueWaypoint(goalPosition);
        }

        /// <summary>
        /// Has goal?
        /// </summary>
        /// <returns></returns>
        public bool HasGoal()
        {
            return !_goal.Equals(Vector2.Zero);
        }

        /// <summary>
        /// Target
        /// </summary>
        private Vector2 _target;

        /// <summary>
        /// Get target
        /// </summary>
        /// <returns></returns>
        public Vector2 GetTarget()
        {
            return _target;
        }

        /// <summary>
        /// Set target position
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Vector2 target)
        {
            _path.Clear();
            _target = target;
        }

        /// <summary>
        /// Path
        /// </summary>
        private readonly Queue<Vector2> _path;

        /// <summary>
        /// Enqueue waypoint
        /// </summary>
        /// <param name="waypointPosition"></param>
        private void EnqueueWaypoint(Vector2 waypointPosition)
        {
            _path.Enqueue(waypointPosition);

            if (_path.Count > 1)
            {
                // Add drawable line (for debugging)
                _drawablePathLines.Add(PrimitiveFactory.Line(Color.Green, _path.ElementAt(_path.Count - 2), _path.Last()));
            }
        }

        /// <summary>
        /// Drawable path lines (for debugging)
        /// </summary>
        private readonly List<LinePrimitive> _drawablePathLines;

        /// <summary>
        /// Movement state
        /// </summary>
        private MovementState _state;

        /// <summary>
        /// Set state
        /// </summary>
        private void SetState(MovementState state)
        {
            _state = state;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MovementBehavior()
        {
            _target = Vector2.Zero;
            _path = new Queue<Vector2>();
            _drawablePathLines = new List<LinePrimitive>();
            _state = MovementState.ApproachTarget;
        }

        /// <summary>
        /// Get force
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public Vector2 GetForce(Vector2 position, Vector2 velocity)
        {
            Vector2 result = Vector2.Zero;

            switch (_state)
            {
                case MovementState.SeekTarget:
                    result = SeekTarget(position);
                    break;
                case MovementState.ApproachTarget:
                    result = ApproachTarget(velocity);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Freeze state
        /// </summary>
        public void Reset()
        {
            _target = Vector2.Zero;
            _path.Clear();
            _drawablePathLines.Clear();
            _state = MovementState.ApproachTarget;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        public void Update(Vector2 position, Vector2 velocity)
        {
            if ((_target - position).Length() < 25f)
            {
                SetState(MovementState.ApproachTarget);
            }
        }

        /// <summary>
        /// Target reached
        /// </summary>
        private void OnTargetReached()
        {
            // If there is at least one item in the path, use it as the next target position
            if (_path != null && _path.Count > 0)
            {
                _target = _path.Dequeue();

                if (_drawablePathLines.Count > 0)
                {
                    _drawablePathLines.RemoveAt(0);
                }

                SetState(MovementState.SeekTarget);
            }
            else
            {
                _goal = Vector2.Zero;
                SetState(MovementState.ApproachTarget);
            }
        }

        /// <summary>
        /// Produces normalized vector used as a force
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector2 SeekTarget(Vector2 position)
        {
            Vector2 positionToTarget = _target - position;
            positionToTarget.Normalize();
            return positionToTarget;
        }

        /// <summary>
        /// Approach target
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        private Vector2 ApproachTarget(Vector2 velocity)
        {
            if (velocity.LengthSquared() < 0.5f)
            {
                OnTargetReached();
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Draw path (useful for debugging)
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            foreach (LinePrimitive drawablePathLine in _drawablePathLines)
            {
                drawablePathLine.Draw(graphicsDevice, basicEffect);
            }
        }
    }
}
