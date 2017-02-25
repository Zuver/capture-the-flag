using Engine.UI.Text.Style;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI.Text
{
    public class LabelBuilder
    {
        /// <summary>
        /// Private data
        /// </summary>
        private Label label;

        /// <summary>
        /// Set alignment
        /// </summary>
        /// <param name="alignment"></param>
        public void SetAlignment(Alignment alignment)
        {
            this.label.SetAlignment(alignment);
        }

        /// <summary>
        /// To label
        /// </summary>
        /// <returns></returns>
        public Label ToLabel()
        {
            return this.label;
        }
    }
}