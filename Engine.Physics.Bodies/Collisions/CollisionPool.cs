using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions.EventHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Physics.Bodies.Collisions
{
    public sealed class CollisionPool
    {
        /// <summary>
        /// This is a singleton
        /// </summary>
        private static CollisionPool _instance;
        public static CollisionPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollisionPool();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Hash set of all collidable entities
        /// </summary>
        private HashSet<AbstractBody> _bodies;

        /// <summary>
        /// Bodies marked for addition
        /// </summary>
        private Queue<AbstractBody> _bodiesMarkedForAddition;

        /// <summary>
        /// Bodies that are marked for removal
        /// </summary>
        private Queue<AbstractBody> _bodiesMarkedForRemoval;

        /// <summary>
        /// Collision detected event
        /// </summary>
        public event CollisionDetectedEventHandler CollisionDetected;

        /// <summary>
        /// Constructor
        /// </summary>
        private CollisionPool()
        {
            this._bodies = new HashSet<AbstractBody>();
            this._bodiesMarkedForAddition = new Queue<AbstractBody>();
            this._bodiesMarkedForRemoval = new Queue<AbstractBody>();
        }

        /// <summary>
        /// Add body
        /// </summary>
        /// <param name="body"></param>
        public void AddBody(AbstractBody body)
        {
            if (!this._bodies.Contains(body))
            {
                this._bodiesMarkedForAddition.Enqueue(body);
            }
        }

        /// <summary>
        /// Remove body
        /// </summary>
        /// <param name="body"></param>
        public void RemoveBody(AbstractBody body)
        {
            if (this._bodies.Contains(body))
            {
                this._bodiesMarkedForRemoval.Enqueue(body);
            }
        }

        /// <summary>
        /// Runs collision detection algorithms and fires off collision events in the event of a collision
        /// </summary>
        private void RunCollisionDetection()
        {
            // Used to keep track of collisions we have already observed during the lifespan of this method invocation
            Dictionary<AbstractBody, List<AbstractBody>> collisionsObserved = new Dictionary<AbstractBody, List<AbstractBody>>();

            foreach (AbstractBody body1 in this._bodies)
            {
                if (!collisionsObserved.ContainsKey(body1))
                {
                    collisionsObserved.Add(body1, new List<AbstractBody>());
                }

                foreach (AbstractBody body2 in this._bodies)
                {
                    if (body1 != body2 && !collisionsObserved[body1].Contains(body2))
                    {
                        // If a collision occurred between the two bodies
                        if (body1.DetectCollision(body2))
                        {
                            // Fire collision event
                            CollisionDetected(new CollisionDetectedEventArgs(body1, body2));

                            // Make sure we aren't making redundant checks for collisions
                            collisionsObserved[body1].Add(body2);

                            if (!collisionsObserved.ContainsKey(body2))
                            {
                                collisionsObserved.Add(body2, new List<AbstractBody>());
                                collisionsObserved[body2].Add(body1);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            this.RunCollisionDetection();

            // Remove bodies that have been marked for removal
            while (this._bodiesMarkedForRemoval.Count > 0)
            {
                AbstractBody body = this._bodiesMarkedForRemoval.Dequeue();

                if (this._bodies.Contains(body))
                {
                    this._bodies.Remove(body);
                }
            }

            // Add bodies that have been marked for addition
            while (this._bodiesMarkedForAddition.Count > 0)
            {
                AbstractBody body = this._bodiesMarkedForAddition.Dequeue();

                if (!this._bodies.Contains(body))
                {
                    this._bodies.Add(body);
                }
            }
        }
    }
}