using CaptureTheFlag.Entities.Walls;
using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaptureTheFlag.Entities.Bullets
{
    public class Bullet : AbstractBullet
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="gun"></param>
        public Bullet(AbstractBody body, PrimitiveBuilder model, AbstractGun gun)
            : base(body, model, gun)
        {
            this.SetCollisionBehaviors(new CollisionBehaviors(this));
        }
    }
}