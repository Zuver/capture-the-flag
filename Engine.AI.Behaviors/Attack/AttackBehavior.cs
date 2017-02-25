using Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.AI.Behaviors.Attack
{
    public class AttackBehavior
    {
        /// <summary>
        /// Target
        /// </summary>
        public AbstractPlayer Target;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttackBehavior()
        {
            this.Target = null;
        }

        /// <summary>
        /// Reset state
        /// </summary>
        public void Reset()
        {
            this.Target = null;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gun"></param>
        /// <param name="canSeeTarget"></param>
        public void Update(Vector2 position, AbstractGun gun, bool canSeeTarget)
        {
            if (this.Target != null && gun != null && canSeeTarget)
            {
                Vector2 toTarget = this.Target.GetBody().GetPosition() - position;
                gun.Fire(toTarget);
            }
        }
    }
}