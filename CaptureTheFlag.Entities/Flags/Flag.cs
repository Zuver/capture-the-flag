using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Flags
{
    public class Flag : AbstractFlag
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        public Flag(AbstractBody body, PrimitiveBuilder model, AbstractTeam team)
            : base(body, model, team)
        {
            this.SetCollisionBehaviors(new CollisionBehaviors(this));
        }

        /// <summary>
        /// Default body
        /// </summary>
        /// <param name="initialPosition"></param>
        /// <returns></returns>
        public static AbstractBody DefaultBody(Vector2 initialPosition)
        {
            return BodyFactory.Circle(false, 1.0f, 1.0f, 0.5f, true, 10f).SetPosition(initialPosition);
        }

        /// <summary>
        /// Default model
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static PrimitiveBuilder DefaultModel(Color color)
        {
            PrimitiveBuilder result = new PrimitiveBuilder(color);
            result.Add(PrimitiveFactory.Circle(color, 10f, 3));
            return result;
        }
    }
}