using Engine.Drawing;
using Engine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class AbstractWall : AbstractEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        public AbstractWall(AbstractBody body, PrimitiveBuilder model)
            : base(body, model)
        {
            EntityTable.Instance.Add(this.Body, this);
        }
    }
}