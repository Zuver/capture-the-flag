using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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