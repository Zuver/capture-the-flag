using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Bullets
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Bullet
        /// </summary>
        public AbstractBullet Bullet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bullet"></param>
        public CollisionBehaviors(AbstractBullet bullet)
        {
            this.Bullet = bullet;
        }

        /// <summary>
        /// React to a collision with a player
        /// </summary>
        /// <param name="player"></param>
        public void ReactToCollision(AbstractPlayer player)
        {
            // Undefined
        }

        /// <summary>
        /// React to a collision with a gun
        /// </summary>
        /// <param name="gun"></param>
        public void ReactToCollision(AbstractGun gun)
        {
            // Undefined
        }

        /// <summary>
        /// React to a collision with another bullet
        /// </summary>
        /// <param name="bullet"></param>
        public void ReactToCollision(AbstractBullet bullet)
        {
            // Undefined
        }

        /// <summary>
        /// React to a collsion with a wall
        /// </summary>
        /// <param name="wall"></param>
        public void ReactToCollision(AbstractWall wall)
        {
            this.Bullet.Destroy();
        }
    }
}