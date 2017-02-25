using Engine.UI.Labels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI.Buttons
{
    public abstract class AbstractButton
    {
        /// <summary>
        /// Protected data
        /// </summary>
        protected Color HoverColor;
        protected bool WasHoveredOnPreviousUpdate;
        protected bool WasMouseClickedOnPreviousUpdate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hoverColor"></param>
        public AbstractButton(Color hoverColor)
        {
            this.HoverColor = hoverColor;
            this.WasHoveredOnPreviousUpdate = false;
            this.WasMouseClickedOnPreviousUpdate = false;
        }

        /// <summary>
        /// Public abstract methods
        /// </summary>
        public abstract void Update();
        public abstract bool IsHovered(int x, int y);
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Protected abstract methods
        /// </summary>
        protected abstract void OnHover(EventArgs e);
        protected abstract void OnHoverExit(EventArgs e);
        protected abstract void OnClick(EventArgs e);
    }
}