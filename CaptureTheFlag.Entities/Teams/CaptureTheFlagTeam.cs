using CaptureTheFlag.Entities.Flags;
using Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Teams
{
    public class CaptureTheFlagTeam : AbstractTeam
    {
        /// <summary>
        /// Flag
        /// </summary>
        private Flag Flag;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="basePosition"></param>
        /// <param name="color"></param>
        public CaptureTheFlagTeam(string name, Vector2 basePosition, Color color)
            : base(name, basePosition, color)
        {
            this.Flag = new Flag(Flag.DefaultBody(basePosition), Flag.DefaultModel(color), this);
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            this.Flag.Update();
        }

        /// <summary>
        /// Get flag
        /// </summary>
        /// <returns></returns>
        public override AbstractFlag GetFlag()
        {
            return this.Flag;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            base.Draw(graphicsDevice, basicEffect, spriteBatch);
            this.Flag.Draw(graphicsDevice, basicEffect, spriteBatch);
        }
    }
}