using Engine.Entities;
using Microsoft.Xna.Framework;

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
            Target = null;
        }

        /// <summary>
        /// Freeze state
        /// </summary>
        public void Reset()
        {
            Target = null;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gun"></param>
        /// <param name="canSeeTarget"></param>
        public void Update(Vector2 position, AbstractGun gun, bool canSeeTarget)
        {
            if (Target != null && gun != null && canSeeTarget)
            {
                Vector2 toTarget = Target.GetBody().GetPosition() - position;
                gun.Fire(toTarget);
            }
        }
    }
}
