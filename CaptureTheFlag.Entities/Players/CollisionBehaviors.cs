using Engine.Entities;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Players
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Player
        /// </summary>
        private AbstractPlayer Player;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="player"></param>
        public CollisionBehaviors(AbstractPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// React to a collision with another player
        /// </summary>
        /// <param name="player"></param>
        public void ReactToCollision(AbstractPlayer player)
        {
            // TODO: Fix this
            
            //Vector2 closestPoint = player.GetBody().GetClosestPointOnPerimeter(this.Player.GetBody().GetPosition());
            //Vector2 closestPointToPosition = this.Player.GetBody().GetPosition() - closestPoint;
            //closestPointToPosition.Normalize();

            //if (float.IsNaN(closestPointToPosition.X))
            //    closestPointToPosition.X = 0f;
            //if (float.IsNaN(closestPointToPosition.Y))
            //    closestPointToPosition.Y = 0f;

            //this.Player.GetBody().SetPosition(closestPoint + closestPointToPosition * ((CircleBody)this.Player.GetBody()).GetRadius());
        }

        /// <summary>
        /// React to a collision with a gun
        /// </summary>
        /// <param name="gun"></param>
        public void ReactToCollision(AbstractGun gun)
        {
            this.Player.PickUpGun(gun);
        }

        /// <summary>
        /// React to a collision with a bullet
        /// </summary>
        /// <param name="bullet"></param>
        public void ReactToCollision(AbstractBullet bullet)
        {
            if (bullet.GetGun() != this.Player.GetGun())
            {
                this.Player.DecreaseHealth(5);
                bullet.Destroy();
            }
        }

        /// <summary>
        /// React to a collision with a wall
        /// </summary>
        /// <param name="wall"></param>
        public void ReactToCollision(AbstractWall wall)
        {
            Vector2 closestPoint = wall.GetBody().GetClosestPointOnPerimeter(this.Player.GetBody().GetPosition());
            Vector2 closestPointToPosition = this.Player.GetBody().GetPosition() - closestPoint;
            closestPointToPosition.Normalize();

            if (float.IsNaN(closestPointToPosition.X))
                closestPointToPosition.X = 0f;
            if (float.IsNaN(closestPointToPosition.Y))
                closestPointToPosition.Y = 0f;

            this.Player.GetBody().SetPosition(closestPoint + closestPointToPosition * ((CircleBody)this.Player.GetBody()).GetRadius());
        }
    }
}