using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI.Labels
{
    public class TextLabelQueue
    {
        /// <summary>
        /// Text label
        /// </summary>
        private TextLabel _textLabel;

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this._textLabel.SetPosition(position);
        }

        /// <summary>
        /// Messages
        /// </summary>
        private Queue<string> _messages;

        /// <summary>
        /// Last dequeue time
        /// </summary>
        private double _lastDequeueTime;

        /// <summary>
        /// Display time
        /// </summary>
        private double _displayTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="spriteFont"></param>
        /// <param name="position"></param>
        /// <param name="alignment"></param>
        /// <param name="color"></param>
        /// <param name="displayTime"></param>
        public TextLabelQueue(SpriteFont spriteFont, Vector2 position, LabelAlignment alignment, Color color, int displayTime)
        {
            this._textLabel = new TextLabel(String.Empty, spriteFont, position, alignment, color);
            this._messages = new Queue<string>();
            this._displayTime = displayTime;
        }

        /// <summary>
        /// Enqueue
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(string message)
        {
            this._messages.Enqueue(message);
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            double currentTime = TimeHelper.GetCurrentGameTime();

            if (currentTime - this._lastDequeueTime > this._displayTime)
            {
                if (this._messages.Count > 0)
                {
                    this._textLabel.SetText(this._messages.Dequeue());
                    this._textLabel.Toast(this._displayTime / 4, this._displayTime / 2);
                    this._lastDequeueTime = currentTime;
                }
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            this._textLabel.Draw(spriteBatch);
        }
    }
}