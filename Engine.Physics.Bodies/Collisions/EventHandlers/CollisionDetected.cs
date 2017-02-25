using Engine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Physics.Bodies.Collisions.EventHandlers
{
    public delegate void CollisionDetectedEventHandler(CollisionDetectedEventArgs e);

    public class CollisionDetectedEventArgs : EventArgs
    {
        public AbstractBody Body1 { get; private set; }
        public AbstractBody Body2 { get; private set; }

        public CollisionDetectedEventArgs(AbstractBody body1, AbstractBody body2)
        {
            this.Body1 = body1;
            this.Body2 = body2;
        }
    }
}