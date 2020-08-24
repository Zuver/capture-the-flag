using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;

namespace CaptureTheFlag.Entities.Walls
{
    public class LineWall : AbstractWall
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        public LineWall(AbstractBody body, PrimitiveBuilder model)
            : base(body, model)
        {
        }
    }
}