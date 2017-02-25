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