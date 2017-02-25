using Engine.Drawing;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public class AbstractBullet : AbstractEntity
    {
        /// <summary>
        /// Is active
        /// </summary>
        protected bool IsActive;

        /// <summary>
        /// Gun
        /// </summary>
        protected AbstractGun Gun;

        /// <summary>
        /// Get gun
        /// </summary>
        /// <returns></returns>
        public AbstractGun GetGun()
        {
            return this.Gun;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="gun"></param>
        public AbstractBullet(AbstractBody body, PrimitiveBuilder model, AbstractGun gun)
            : base(body, model)
        {
            this.IsActive = false;
            this.Gun = gun;
            EntityTable.Instance.Add(this.Body, this);
        }

        /// <summary>
        /// Fire
        /// </summary>
        /// <param name="from"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        public void Fire(Vector2 from, Vector2 direction, float speed)
        {
            this.IsActive = true;
            CollisionPool.Instance.AddBody(this.Body);

            this.Body.SetPosition(from);

            direction.Normalize();
            this.Body.SetVelocity(speed * direction);
        }

        /// <summary>
        /// Destroy
        /// </summary>
        public void Destroy()
        {
            this.IsActive = false;
            CollisionPool.Instance.RemoveBody(this.Body);
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update(Vector2 position)
        {
            base.Update();

            if (!this.IsActive)
            {
                this.Body.SetPosition(position);
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {
                base.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }
    }
}