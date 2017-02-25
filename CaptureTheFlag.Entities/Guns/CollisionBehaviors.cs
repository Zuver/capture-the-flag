using Engine.Entities;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Guns
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Gun
        /// </summary>
        private AbstractGun Gun;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gun"></param>
        public CollisionBehaviors(AbstractGun gun)
        {
            this.Gun = gun;
        }

        /// <summary>
        /// React to collision with a player
        /// </summary>
        /// <param name="player"></param>
        public void ReactToCollision(AbstractPlayer player)
        {
            // Undefined
        }

        /// <summary>
        /// React to collision with another gun
        /// </summary>
        /// <param name="gun"></param>
        public void ReactToCollision(AbstractGun gun)
        {
            // Undefined
        }

        /// <summary>
        /// React to collision with a bullet
        /// </summary>
        /// <param name="bullet"></param>
        public void ReactToCollision(AbstractBullet bullet)
        {
            // Undefined
        }

        /// <summary>
        /// React to collision with a wall
        /// </summary>
        /// <param name="wall"></param>
        public void ReactToCollision(AbstractWall wall)
        {
            Vector2 closestPoint = wall.GetBody().GetClosestPointOnPerimeter(this.Gun.GetBody().GetPosition());
            Vector2 closestPointToPosition = this.Gun.GetBody().GetPosition() - closestPoint;
            closestPointToPosition.Normalize();

            if (float.IsNaN(closestPointToPosition.X))
                closestPointToPosition.X = 0f;
            if (float.IsNaN(closestPointToPosition.Y))
                closestPointToPosition.Y = 0f;

            this.Gun.GetBody().SetPosition(closestPoint + closestPointToPosition * ((CircleBody)this.Gun.GetBody()).GetRadius());
        }
    }
}