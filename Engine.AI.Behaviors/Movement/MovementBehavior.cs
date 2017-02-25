using Engine.Drawing;
using Engine.Entities;
using Engine.Entities.Graphs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.AI.Behaviors.Movement
{
    public class MovementBehavior
    {
        /// <summary>
        /// Goal
        /// </summary>
        private Vector2 Goal;

        /// <summary>
        /// Get goal
        /// </summary>
        /// <returns></returns>
        public Vector2 GetGoal()
        {
            return this.Goal;
        }

        /// <summary>
        /// Set goal
        /// </summary>
        /// <param name="goalPosition"></param>
        /// <param name="routeToGoal"></param>
        public void SetGoal(Vector2 goalPosition, List<Node> routeToGoal)
        {
            this.Goal = goalPosition;

            foreach (Node node in routeToGoal)
            {
                this.EnqueueWaypoint(node.Position);
            }

            this.EnqueueWaypoint(goalPosition);
        }

        /// <summary>
        /// Has goal?
        /// </summary>
        /// <returns></returns>
        public bool HasGoal()
        {
            return !this.Goal.Equals(Vector2.Zero);
        }

        /// <summary>
        /// Target
        /// </summary>
        private Vector2 Target;

        /// <summary>
        /// Get target
        /// </summary>
        /// <returns></returns>
        public Vector2 GetTarget()
        {
            return this.Target;
        }

        /// <summary>
        /// Set target position
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Vector2 target)
        {
            this.Path.Clear();
            this.Target = target;
        }

        /// <summary>
        /// Path
        /// </summary>
        private Queue<Vector2> Path;

        /// <summary>
        /// Enqueue waypoint
        /// </summary>
        /// <param name="waypointPosition"></param>
        private void EnqueueWaypoint(Vector2 waypointPosition)
        {
            this.Path.Enqueue(waypointPosition);

            if (this.Path.Count > 1)
            {
                // Add drawable line (for debugging)
                this.DrawablePathLines.Add(PrimitiveFactory.Line(Color.Green, this.Path.ElementAt(this.Path.Count - 2), this.Path.Last()));
            }
        }

        /// <summary>
        /// Drawable path lines (for debugging)
        /// </summary>
        private List<LinePrimitive> DrawablePathLines;

        /// <summary>
        /// Movement state
        /// </summary>
        private MovementState State;

        /// <summary>
        /// Set state
        /// </summary>
        private void SetState(MovementState state)
        {
            this.State = state;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MovementBehavior()
        {
            this.Target = Vector2.Zero;
            this.Path = new Queue<Vector2>();
            this.DrawablePathLines = new List<LinePrimitive>();
            this.State = MovementState.ApproachTarget;
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

            switch (this.State)
            {
                case MovementState.SeekTarget:
                    result = this.SeekTarget(position);
                    break;
                case MovementState.ApproachTarget:
                    result = this.ApproachTarget(position, velocity);
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Reset state
        /// </summary>
        public void Reset()
        {
            this.Target = Vector2.Zero;
            this.Path.Clear();
            this.DrawablePathLines.Clear();
            this.State = MovementState.ApproachTarget;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        public void Update(Vector2 position, Vector2 velocity)
        {
            if ((this.Target - position).Length() < 25f)
            {
                this.SetState(MovementState.ApproachTarget);
            }
        }

        /// <summary>
        /// Target reached
        /// </summary>
        private void OnTargetReached(Vector2 position)
        {
            // If there is at least one item in the path, use it as the next target position
            if (this.Path != null && this.Path.Count > 0)
            {
                this.Target = this.Path.Dequeue();

                if (this.DrawablePathLines.Count > 0)
                {
                    this.DrawablePathLines.RemoveAt(0);
                }

                this.SetState(MovementState.SeekTarget);
            }
            else
            {
                this.Goal = Vector2.Zero;
                this.SetState(MovementState.ApproachTarget);
            }
        }

        /// <summary>
        /// Produces normalized vector used as a force
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector2 SeekTarget(Vector2 position)
        {
            Vector2 positionToTarget = this.Target - position;
            positionToTarget.Normalize();
            return positionToTarget;
        }

        /// <summary>
        /// Approach target
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        private Vector2 ApproachTarget(Vector2 position, Vector2 velocity)
        {
            if (velocity.LengthSquared() < 0.5f)
            {
                this.OnTargetReached(position);
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
            foreach (LinePrimitive drawablePathLine in this.DrawablePathLines)
            {
                drawablePathLine.Draw(graphicsDevice, basicEffect);
            }
        }
    }
}
