using Engine.Entities;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;

namespace CaptureTheFlag.Entities.Guns
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Gun
        /// </summary>
        private readonly AbstractGun _gun;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gun"></param>
        public CollisionBehaviors(AbstractGun gun)
        {
            _gun = gun;
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
            Vector2 closestPoint = wall.GetBody().GetClosestPointOnPerimeter(_gun.GetBody().GetPosition());
            Vector2 closestPointToPosition = _gun.GetBody().GetPosition() - closestPoint;
            closestPointToPosition.Normalize();

            if (float.IsNaN(closestPointToPosition.X))
                closestPointToPosition.X = 0f;
            if (float.IsNaN(closestPointToPosition.Y))
                closestPointToPosition.Y = 0f;

            _gun.GetBody().SetPosition(closestPoint + closestPointToPosition * ((CircleBody)_gun.GetBody()).GetRadius());
        }
    }
}