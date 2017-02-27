using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;

namespace CaptureTheFlag.Entities.Walls
{
    public class CircleWall : AbstractWall
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        public CircleWall(AbstractBody body, PrimitiveBuilder model)
            : base(body, model)
        {
        }
    }
}
