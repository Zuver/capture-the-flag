using Engine.Drawing;
using Engine.Entities.Graphs;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Engine.Physics.Bodies.Collisions.EventHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Entities
{
    public abstract class AbstractEntity
    {
        /// <summary>
        /// Body for interaction
        /// </summary>
        protected AbstractBody Body { get; set; }

        /// <summary>
        /// Get body
        /// </summary>
        /// <returns></returns>
        public AbstractBody GetBody()
        {
            return this.Body;
        }

        /// <summary>
        /// Model for display
        /// </summary>
        private readonly AbstractPrimitive _model;

        /// <summary>
        /// Collision behaviors
        /// </summary>
        private ICollisionBehaviors _collisionBehaviors;

        /// <summary>
        /// Set collision behaviors
        /// </summary>
        /// <param name="collisionBehaviors"></param>
        public void SetCollisionBehaviors(ICollisionBehaviors collisionBehaviors)
        {
            this._collisionBehaviors = collisionBehaviors;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="collisionBehaviors"></param>
        public AbstractEntity(AbstractBody body, PrimitiveBuilder model, ICollisionBehaviors collisionBehaviors = null)
        {
            // Set body
            this.Body = body;

            // Set model
            this._model = model;

            // Subscribe to collision event
            CollisionPool.Instance.CollisionDetected += CollisionDetected;

            // Set collision behaviors
            this._collisionBehaviors = collisionBehaviors;
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update()
        {
            this.Body.Update();
            this._model.SetPosition(this.Body.GetPosition());
        }

        /// <summary>
        /// Look
        /// </summary>
        /// <param name="where"></param>
        public void Look(Vector2 where)
        {
            this._model.Rotate((float)Math.Atan2(where.Y, where.X));
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            if (this._model != null)
            {
                this._model.Draw(graphicsDevice, basicEffect);
            }
        }

        /// <summary>
        /// This method is executed when some body has collided with another body
        /// </summary>
        /// <param name="e"></param>
        private void CollisionDetected(CollisionDetectedEventArgs e)
        {
            if (e.Body1 == this.Body)
            {
                this.ReactToCollision(EntityTable.Instance.Get(e.Body2));
            }
            else if (e.Body2 == this.Body)
            {
                this.ReactToCollision(EntityTable.Instance.Get(e.Body1));
            }
        }

        /// <summary>
        /// React to a collision with the given entity
        /// </summary>
        /// <param name="entity"></param>
        private void ReactToCollision(AbstractEntity entity)
        {
            System.Type entityType = entity.GetType();

            if (this._collisionBehaviors != null)
            {
                if (entityType.IsSubclassOf(typeof(AbstractPlayer)))
                {
                    this._collisionBehaviors.ReactToCollision((AbstractPlayer)entity);
                }
                else if (entityType.IsSubclassOf(typeof(AbstractGun)))
                {
                    this._collisionBehaviors.ReactToCollision((AbstractGun)entity);
                }
                else if (entityType.IsSubclassOf(typeof(AbstractBullet)))
                {
                    this._collisionBehaviors.ReactToCollision((AbstractBullet)entity);
                }
                else if (entityType.IsSubclassOf(typeof(AbstractWall)))
                {
                    this._collisionBehaviors.ReactToCollision((AbstractWall)entity);
                }
            }
        }
    }
}