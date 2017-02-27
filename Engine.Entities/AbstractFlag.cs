using Engine.Drawing;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class AbstractFlag : AbstractEntity
    {
        /// <summary>
        /// Team
        /// </summary>
        protected AbstractTeam Team;

        /// <summary>
        /// Get team
        /// </summary>
        /// <returns></returns>
        public AbstractTeam GetTeam()
        {
            return Team;
        }

        /// <summary>
        /// Initial position
        /// </summary>
        protected Vector2 InitialPosition;

        /// <summary>
        /// Freeze position
        /// </summary>
        public void ResetPosition()
        {
            Body.SetPosition(InitialPosition);
        }

        /// <summary>
        /// Is reset
        /// </summary>
        /// <returns></returns>
        public bool IsReset()
        {
            return (InitialPosition - Body.GetPosition()).LengthSquared() < float.Epsilon;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        public AbstractFlag(AbstractBody body, PrimitiveBuilder model, AbstractTeam team)
            : base(body, model)
        {
            Team = team;
            InitialPosition = body.GetPosition();
            EntityTable.Instance.Add(Body, this);
        }
    }
}
