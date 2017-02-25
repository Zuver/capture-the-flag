using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.AI.Behaviors.Movement
{
    public enum MovementState
    {
        SeekTarget,
        ApproachTarget // Player should slow down a little as the target is approached
    }
}