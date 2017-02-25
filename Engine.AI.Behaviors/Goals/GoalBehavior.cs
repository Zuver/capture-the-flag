using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.AI.Behaviors.Goals
{
    public class GoalBehavior
    {
        /// <summary>
        /// Goal state
        /// </summary>
        private GoalState _goalState;

        /// <summary>
        /// Set goal state
        /// </summary>
        /// <param name="goalState"></param>
        public void SetGoalState(GoalState goalState)
        {
            this._goalState = goalState;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GoalBehavior()
        {
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
        }
    }
}