using Engine.Drawing;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return this.Team;
        }

        /// <summary>
        /// Initial position
        /// </summary>
        protected Vector2 InitialPosition;

        /// <summary>
        /// Reset position
        /// </summary>
        public void ResetPosition()
        {
            this.Body.SetPosition(this.InitialPosition);
        }

        /// <summary>
        /// Is reset
        /// </summary>
        /// <returns></returns>
        public bool IsReset()
        {
            return (this.InitialPosition - this.Body.GetPosition()).LengthSquared() < float.Epsilon;
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
            this.Team = team;
            this.InitialPosition = body.GetPosition();
            EntityTable.Instance.Add(this.Body, this);
        }
    }
}