using Engine.Drawing;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class AbstractGun : AbstractEntity
    {
        #region Abstract methods

        /// <summary>
        /// Fire gun
        /// </summary>
        /// <param name="direction"></param>
        public abstract void Fire(Vector2 direction);

        #endregion Abstract methods

        /// <summary>
        /// Player
        /// </summary>
        protected AbstractPlayer Player;

        /// <summary>
        /// Has owner
        /// </summary>
        /// <returns></returns>
        public bool HasOwner()
        {
            return this.Player != null;
        }

        /// <summary>
        /// Set player
        /// </summary>
        public void SetPlayer(AbstractPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Bullets
        /// </summary>
        protected AbstractBullet[] Bullets;

        /// <summary>
        /// Set bullets
        /// </summary>
        /// <param name="bullets"></param>
        public void SetBullets(AbstractBullet[] bullets)
        {
            this.Bullets = bullets;
        }

        /// <summary>
        /// Bullet index
        /// </summary>
        protected int BulletIndex;

        /// <summary>
        /// Delay in milliseconds
        /// </summary>
        protected double DelayInMilliseconds;

        /// <summary>
        /// Last time bullet fired
        /// </summary>
        protected double LastTimeBulletFired;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="delayInMilliseconds"></param>
        public AbstractGun(AbstractBody body, PrimitiveBuilder model, double delayInMilliseconds)
            : base(body, model)
        {
            this.BulletIndex = 0;
            this.DelayInMilliseconds = delayInMilliseconds;

            // Add Entity table entry that maps the Body to this Entity
            EntityTable.Instance.Add(this.Body, this);
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            foreach (AbstractBullet bullet in this.Bullets)
            {
                bullet.Update(this.Body.GetPosition());
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
            base.Draw(graphicsDevice, basicEffect, spriteBatch);

            foreach (AbstractBullet bullet in this.Bullets)
            {
                bullet.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }

        /// <summary>
        /// Increment bullet index
        /// </summary>
        protected void IncrementBulletIndex()
        {
            this.BulletIndex++;

            if (this.BulletIndex >= this.Bullets.Length)
            {
                this.BulletIndex = 0;
            }
        }
    }
}