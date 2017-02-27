using Engine.Drawing;
using Engine.Physics.Bodies;

namespace Engine.Entities
{
    public abstract class AbstractWall : AbstractEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        protected AbstractWall(AbstractBody body, PrimitiveBuilder model)
            : base(body, model)
        {
            EntityTable.Instance.Add(Body, this);
        }
    }
}
