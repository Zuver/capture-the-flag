using Engine.UI.Labels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI.Buttons
{
    public class TextButton : AbstractButton
    {
        /// <summary>
        /// These events can be subscribed to by users of this class
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Private data
        /// </summary>
        private TextLabel Label;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label">Label</param>
        public TextButton(TextLabel label, Color hoverColor)
            : base(hoverColor)
        {
            this.Label = label;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            MouseState mouseState = Mouse.GetState();

            // Check if this button is being hovered by the mouse
            if (!this.WasMouseClickedOnPreviousUpdate && IsHovered(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
            {
                // Invoke OnClick event
                OnClick(EventArgs.Empty);
            }

            this.WasMouseClickedOnPreviousUpdate = (bool)(mouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the given x and y coordinates are inside
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <returns>True if the given x and y coordinates are hovering this button</returns>
        public override bool IsHovered(int x, int y)
        {
            bool isHovered = false;

            isHovered = this.Label.TopLeftPosition.X < x &&
                this.Label.TopLeftPosition.Y < y &&
                this.Label.TopLeftPosition.X + this.Label.Size.X > x &&
                this.Label.TopLeftPosition.Y + this.Label.Size.Y > y;

            if (isHovered)
            {
                this.OnHover(EventArgs.Empty);
                this.WasHoveredOnPreviousUpdate = true;
            }
            else if (this.WasHoveredOnPreviousUpdate)
            {
                this.OnHoverExit(EventArgs.Empty);
                this.WasHoveredOnPreviousUpdate = false;
            }

            return isHovered;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.Label.Draw(spriteBatch);
        }

        /// <summary>
        /// This method is called when this button has been hovered
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHover(EventArgs e)
        {
            this.Label.RenderColor = this.HoverColor;
        }

        /// <summary>
        /// This method is called when this button is no longer hovered
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHoverExit(EventArgs e)
        {
            this.Label.RenderColor = this.Label.DefaultColor;
        }

        /// <summary>
        /// This method is called when this button has been clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            this.Clicked(this, e);
        }
    }
}