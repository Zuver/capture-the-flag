using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Flags
{
    public class CollisionBehaviors : ICollisionBehaviors
    {
        /// <summary>
        /// Flag
        /// </summary>
        private AbstractFlag _flag;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flag"></param>
        public CollisionBehaviors(AbstractFlag flag)
        {
            this._flag = flag;
        }

        /// <summary>
        /// React to a collision with a player
        /// </summary>
        /// <param name="player"></param>
        public void ReactToCollision(AbstractPlayer player)
        {
            if (!this._flag.IsReset() && this._flag.GetTeam() == player.GetTeam())
            {
                player.ReturnFlag(this._flag);
            }
            else if (this._flag.GetTeam() != player.GetTeam())
            {
                player.PickUpFlag(this._flag);
            }
            else if (player.HasFlag() && player.GetTeam() == this._flag.GetTeam())
            {
                player.CaptureFlag();
            }
        }

        /// <summary>
        /// React to a collision with a gun
        /// </summary>
        /// <param name="gun"></param>
        public void ReactToCollision(AbstractGun gun)
        {
            // TODO
        }

        /// <summary>
        /// React to a collision with a bullet
        /// </summary>
        /// <param name="bullet"></param>
        public void ReactToCollision(AbstractBullet bullet)
        {
            // TODO
        }

        /// <summary>
        /// React to a collision with a wall
        /// </summary>
        /// <param name="wall"></param>
        public void ReactToCollision(AbstractWall wall)
        {
            // TODO
        }
    }
}