using CaptureTheFlag.Entities.Bullets;
using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;

namespace CaptureTheFlag.Entities.Guns
{
    public class WeakGun : AbstractGun
    {
        // Tweak these to change the game experience
        private static double BulletDelayInMilliseconds = 150.0;
        private static float BulletSpeed = 10.0f;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        public WeakGun(AbstractBody body, PrimitiveBuilder model)
            : base(body, model, BulletDelayInMilliseconds)
        {
            AbstractBullet[] bullets = new AbstractBullet[100];
            for (int i = 0; i < bullets.Length; i++)
            {
                PrimitiveBuilder bulletModel = new PrimitiveBuilder(Color.Yellow);
                bulletModel.Add(PrimitiveFactory.Circle(Color.Yellow, 1f, 5));
                bullets[i] = new Bullet(BodyFactory.Circle(false, 1f, BulletSpeed, 1f, false, 1f), bulletModel, this);
            }

            SetBullets(bullets);
            SetCollisionBehaviors(new CollisionBehaviors(this));
        }

        /// <summary>
        /// Default body
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static AbstractBody DefaultBody(Vector2 position)
        {
            CircleBody result = BodyFactory.Circle(false, 0f, 0f, 0f, true, 6f);
            return result.SetPosition(position);
        }

        /// <summary>
        /// Default model
        /// </summary>
        /// <returns></returns>
        public static PrimitiveBuilder DefaultModel()
        {
            PrimitiveBuilder result = new PrimitiveBuilder(Color.Yellow);
            result.Add(PrimitiveFactory.Circle(Color.Yellow, 6f, 6));
            return result;
        }

        /// <summary>
        /// Fire WeakGun
        /// </summary>
        /// <param name="direction"></param>
        public override void Fire(Vector2 direction)
        {
            // Get current game time
            double currentGameTime = TimeHelper.GetCurrentGameTime();

            // Check for delay
            if (currentGameTime - LastTimeBulletFired > DelayInMilliseconds)
            {
                Bullets[BulletIndex].Fire(Body.GetPosition(), direction, BulletSpeed);
                IncrementBulletIndex();
                LastTimeBulletFired = currentGameTime;
            }
        }
    }
}
