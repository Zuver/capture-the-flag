using CaptureTheFlag.Entities.Flags;
using Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaptureTheFlag.Entities.Teams
{
    public class CaptureTheFlagTeam : AbstractTeam
    {
        /// <summary>
        /// Flag
        /// </summary>
        private readonly Flag _flag;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="basePosition"></param>
        /// <param name="color"></param>
        public CaptureTheFlagTeam(string name, Vector2 basePosition, Color color)
            : base(name, basePosition, color)
        {
            _flag = new Flag(Flag.DefaultBody(basePosition), Flag.DefaultModel(color), this);
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            _flag.Update();
        }

        /// <summary>
        /// Get flag
        /// </summary>
        /// <returns></returns>
        public override AbstractFlag GetFlag()
        {
            return _flag;
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
            _flag.Draw(graphicsDevice, basicEffect, spriteBatch);
        }
    }
}
