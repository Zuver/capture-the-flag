using Engine.Entities;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;

namespace CaptureTheFlag.Entities.Players
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Player
        /// </summary>
        private readonly AbstractPlayer _player;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="player"></param>
        public CollisionBehaviors(AbstractPlayer player)
        {
            _player = player;
        }

        /// <summary>
        /// React to a collision with another player
        /// </summary>
        /// <param name="player"></param>
        public void ReactToCollision(AbstractPlayer player)
        {
            // Players will just go through each other
        }

        /// <summary>
        /// React to a collision with a gun
        /// </summary>
        /// <param name="gun"></param>
        public void ReactToCollision(AbstractGun gun)
        {
            _player.PickUpGun(gun);
        }

        /// <summary>
        /// React to a collision with a bullet
        /// </summary>
        /// <param name="bullet"></param>
        public void ReactToCollision(AbstractBullet bullet)
        {
            if (bullet.GetGun() != _player.GetGun())
            {
                _player.DecreaseHealth(5);
                bullet.Destroy();
            }
        }

        /// <summary>
        /// React to a collision with a wall
        /// </summary>
        /// <param name="wall"></param>
        public void ReactToCollision(AbstractWall wall)
        {
            Vector2 closestPoint = wall.GetBody().GetClosestPointOnPerimeter(_player.GetBody().GetPosition());
            Vector2 closestPointToPosition = _player.GetBody().GetPosition() - closestPoint;
            closestPointToPosition.Normalize();

            if (float.IsNaN(closestPointToPosition.X))
                closestPointToPosition.X = 0f;
            if (float.IsNaN(closestPointToPosition.Y))
                closestPointToPosition.Y = 0f;

            _player.GetBody()
                .SetPosition(closestPoint + closestPointToPosition * ((CircleBody) _player.GetBody()).GetRadius());
        }
    }
}
