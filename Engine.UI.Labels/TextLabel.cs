using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.UI.Labels
{
    public class TextLabel : AbstractLabel
    {
        /// <summary>
        /// Text
        /// </summary>
        protected string Text { get; set; }

        /// <summary>
        /// Set text
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            this.Text = text;
            this.Origin = this.GetOrigin(text);
            this.Size = this.Font.MeasureString(this.Text);
        }

        /// <summary>
        /// Font
        /// </summary>
        public SpriteFont Font { get; private set; }

        // Constructor
        public TextLabel(string text, SpriteFont spriteFont,
            Vector2 position, LabelAlignment alignment, Color color)
            : base(color, alignment)
        {
            this.Text = text;
            this.Font = spriteFont;

            // From base class
            this.Origin = GetOrigin(text);
            this.Size = spriteFont.MeasureString(text);
            this.TopLeftPosition = alignment == LabelAlignment.TopLeft ? position :
                alignment == LabelAlignment.Center ? position - this.Origin :
                Vector2.Zero;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Update();

            spriteBatch.DrawString(this.Font,
                this.Text,
                this.TopLeftPosition,
                this.RenderColor,
                0.0f,
                Vector2.Zero,
                1.0f,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                0.0f);
        }

        /// <summary>
        /// Set font
        /// </summary>
        /// <param name="spriteFont">SpriteFont</param>
        public void SetFont(SpriteFont spriteFont)
        {
            this.Font = spriteFont;
        }

        /// <summary>
        /// Uses the class state to determine the origin
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected Vector2 GetOrigin(string text)
        {
            Vector2 result = Vector2.Zero;

            switch (this.Alignment)
            {
                case LabelAlignment.TopLeft:
                    result = Vector2.Zero;
                    break;
                case LabelAlignment.Center:
                    Microsoft.Xna.Framework.Vector2 textDimensions = this.Font.MeasureString(text);
                    result.X = textDimensions.X / 2;
                    result.Y = textDimensions.Y / 2;
                    break;
            }

            return result;
        }
    }
}