﻿using System;
using CaptureTheFlag.Entities.Bullets;
using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;

namespace CaptureTheFlag.Entities.Guns
{
    public class ShotGun : AbstractGun
    {
        #region Default ShotGun attributes

        // Tweak these to change the game experience
        private static double BulletDelayInMilliseconds = 400.0;
        private static float BulletSpeed = 10.0f;
        private static ushort BulletsPerShot = 7;
        private static double RadiansOfBulletSpread = Math.PI / 12; // Pi / 12 is 15 degrees

        #endregion Default ShotGun attributes

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        public ShotGun(AbstractBody body, PrimitiveBuilder model)
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
        /// Default body for ShotGun
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static AbstractBody DefaultBody(Vector2 position)
        {
            CircleBody result = BodyFactory.Circle(false, 0f, 0f, 0f, true, 6f);

            return result.SetPosition(position);
        }

        /// <summary>
        /// Default model for ShotGun
        /// </summary>
        /// <returns></returns>
        public static PrimitiveBuilder DefaultModel()
        {
            PrimitiveBuilder result = new PrimitiveBuilder(Color.HotPink);
            result.Add(PrimitiveFactory.Circle(Color.HotPink, 6f, 6));

            return result;
        }

        /// <summary>
        /// Fire Shotgun
        /// </summary>
        /// <param name="direction"></param>
        public override void Fire(Vector2 direction)
        {
            // Get current game time
            double currentGameTime = TimeHelper.GetCurrentGameTime();

            // Check for delay
            if (currentGameTime - LastTimeBulletFired > DelayInMilliseconds)
            {
                double directionInRadians = Math.Atan2(direction.Y, direction.X);

                if (directionInRadians < 0)
                    directionInRadians += MathHelper.TwoPi;

                double initialDirection = directionInRadians - (RadiansOfBulletSpread / 2);

                for (int i = 0; i < BulletsPerShot; i++)
                {
                    direction = new Vector2(
                        (float) Math.Cos(initialDirection + (i + 1) * (RadiansOfBulletSpread / (BulletsPerShot + 1))),
                        (float) Math.Sin(initialDirection + (i + 1) * (RadiansOfBulletSpread / (BulletsPerShot + 1))));

                    Bullets[BulletIndex].Fire(Body.GetPosition(), direction, BulletSpeed);
                    IncrementBulletIndex();
                    LastTimeBulletFired = currentGameTime;
                }
            }
        }
    }
}
