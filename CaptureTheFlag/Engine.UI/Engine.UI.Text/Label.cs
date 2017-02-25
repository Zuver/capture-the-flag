using Engine.UI.Text.Style;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI.Text
{
    public class Label
    {
        /// <summary>
        /// Public data
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Private data
        /// </summary>
        private SpriteFont spriteFont;
        private Alignment Alignment;
        private FontSize FontSize;
        private FontStyle FontStyle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public Label(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Set alignment
        /// </summary>
        /// <param name="alignment"></param>
        public void SetAlignment(Alignment alignment)
        {
            this.Alignment = alignment;
        }

        /// <summary>
        /// Set font size
        /// </summary>
        /// <param name="fontSize"></param>
        public void SetFontSize(FontSize fontSize)
        {
            this.FontSize = fontSize;
        }

        /// <summary>
        /// Set font style
        /// </summary>
        /// <param name="fontStyle"></param>
        public void SetFontStyle(FontStyle fontStyle)
        {
            this.FontStyle = fontStyle;
        }
    }
}