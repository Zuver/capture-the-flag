using System.Diagnostics;
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
        /// Reaction time in milliseconds
        /// </summary>
        private readonly float _reactionTimeInMilliseconds;

        /// <summary>
        /// Reaction time StopWatch
        /// </summary>
        private readonly Stopwatch _reactionTimeStopWatch;

        /// <summary>
        /// Was the target visible in the previous frame?
        /// </summary>
        private bool _wasTargetVisibleInPreviousFrame;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttackBehavior(float reactionTimeInMilliseconds)
        {
            Target = null;
            _reactionTimeInMilliseconds = reactionTimeInMilliseconds;
            _reactionTimeStopWatch = new Stopwatch();
            _wasTargetVisibleInPreviousFrame = false;
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
            // This will only execute if the target was not visible in the previous frame but is now visible
            if (!_wasTargetVisibleInPreviousFrame && canSeeTarget)
            {
                _reactionTimeStopWatch.Start();
            }

            // This will execute if the target is not currently visible but was visible in the last frame
            if (_wasTargetVisibleInPreviousFrame && !canSeeTarget)
            {
                _reactionTimeStopWatch.Stop();
                _reactionTimeStopWatch.Reset();
            }

            // Do we have a target and something to shoot it with?
            if (Target != null && gun != null)
            {
                // Determine if the reaction time requirement has been satisfied
                bool isReactionTimeSatisfied = _reactionTimeStopWatch != null &&
                                               _reactionTimeStopWatch.ElapsedMilliseconds >= _reactionTimeInMilliseconds;

                if (isReactionTimeSatisfied)
                {
                    Vector2 toTarget = Target.GetBody().GetPosition() - position;
                    gun.Fire(toTarget);
                }
            }

            _wasTargetVisibleInPreviousFrame = canSeeTarget;
        }
    }
}
