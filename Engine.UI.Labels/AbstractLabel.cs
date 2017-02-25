using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.UI.Labels
{
    public abstract class AbstractLabel
    {
        /// <summary>
        /// Top-left position
        /// </summary>
        public Vector2 TopLeftPosition { get; set; }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            if (this.Alignment == LabelAlignment.Center)
            {
                this.TopLeftPosition = position - this.Origin;
            }
            else if (this.Alignment == LabelAlignment.TopLeft)
            {
                this.TopLeftPosition = position;
            }
        }

        public Vector2 Origin { get; protected set; }
        public Vector2 Size { get; protected set; }
        public Color RenderColor { get; set; }
        public Color DefaultColor { get; set; }
        public LabelAlignment Alignment { get; set; }

        /// <summary>
        /// Label state
        /// </summary>
        private LabelState _labelState;

        /// <summary>
        /// Time when fade in started
        /// </summary>
        private double _timeFadeInStarted;

        /// <summary>
        /// Fade in duration
        /// </summary>
        private double _fadeInDuration;

        /// <summary>
        /// Time when fade out started
        /// </summary>
        private double _timeFadeOutStarted;

        /// <summary>
        /// Fade out duration
        /// </summary>
        private double _fadeOutDuration;

        /// <summary>
        /// Time toast started
        /// </summary>
        private double _timeToastStarted;

        /// <summary>
        /// Toast duration
        /// </summary>
        private double _toastTransitionDuration;

        /// <summary>
        /// Toast visible duration
        /// </summary>
        private double _toastVisibleDuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alignment"></param>
        public AbstractLabel(Color color, LabelAlignment alignment)
        {
            this.RenderColor = color;
            this.DefaultColor = color;
            this.Alignment = alignment;
            this._labelState = LabelState.Visible;
        }

        /// <summary>
        /// Update
        /// </summary>
        protected void Update()
        {
            if (this._labelState == LabelState.FadingIn)
            {
                double currentGameTime = TimeHelper.GetCurrentGameTime();
                int alpha = (int)(((currentGameTime - this._timeFadeInStarted) / this._fadeInDuration) * 255);
                this.RenderColor = new Color(this.RenderColor.R, this.RenderColor.G, this.RenderColor.B, alpha);

                if (alpha >= 255)
                {
                    this._labelState = LabelState.Visible;
                }
            }
            else if (this._labelState == LabelState.FadingOut)
            {
                double currentGameTime = TimeHelper.GetCurrentGameTime();
                int alpha = (255 - (int)(((currentGameTime - this._timeFadeOutStarted) / this._fadeOutDuration) * 255));
                this.RenderColor = new Color(this.RenderColor.R, this.RenderColor.G, this.RenderColor.B, alpha);

                if (alpha <= 0)
                {
                    this._labelState = LabelState.Hidden;
                }
            }
            else if (this._labelState == LabelState.Toast)
            {
                double currentGameTime = TimeHelper.GetCurrentGameTime();
                int alpha;

                if (currentGameTime - this._timeToastStarted < this._toastTransitionDuration)
                {
                    alpha = (int)(((currentGameTime - this._timeToastStarted) / this._toastTransitionDuration) * 255);
                }
                else if (currentGameTime - this._timeToastStarted < this._toastTransitionDuration + this._toastVisibleDuration)
                {
                    alpha = 255;
                }
                else
                {
                    alpha = (255 - (int)(((currentGameTime - this._timeToastStarted - this._toastTransitionDuration - this._toastVisibleDuration) / this._toastTransitionDuration) * 255));

                    if (alpha <= 0)
                    {
                        this._labelState = LabelState.Hidden;
                    }
                }

                this.RenderColor = new Color(this.RenderColor.R, this.RenderColor.G, this.RenderColor.B, alpha);
            }
        }

        /// <summary>
        /// Toast
        /// </summary>
        /// <param name="transitionDuration"></param>
        /// <param name="visibleDuration"></param>
        public void Toast(double transitionDuration, double visibleDuration)
        {
            if (this._labelState != LabelState.FadingIn && this._labelState != LabelState.FadingOut)
            {
                this._labelState = LabelState.Toast;
                this._toastTransitionDuration = transitionDuration;
                this._toastVisibleDuration = visibleDuration;
                this._timeToastStarted = TimeHelper.GetCurrentGameTime();
            }
        }

        /// <summary>
        /// Fade in
        /// </summary>
        /// <param name="milliseconds"></param>
        public void FadeIn(double milliseconds)
        {
            if (this._labelState != LabelState.FadingOut)
            {
                this._labelState = LabelState.FadingIn;
                this._fadeInDuration = milliseconds;
                this._timeFadeInStarted = TimeHelper.GetCurrentGameTime();
            }
        }

        /// <summary>
        /// Fade out
        /// </summary>
        /// <param name="milliseconds"></param>
        public void FadeOut(double milliseconds)
        {
            if (this._labelState != LabelState.FadingIn)
            {
                this._labelState = LabelState.FadingOut;
                this._fadeOutDuration = milliseconds;
                this._timeFadeOutStarted = TimeHelper.GetCurrentGameTime();
            }
        }

        /// <summary>
        /// Toggle fade
        /// </summary>
        /// <param name="milliseconds"></param>
        public void ToggleFade(double milliseconds)
        {
            if (this._labelState == LabelState.Visible)
            {
                this.FadeOut(milliseconds);
            }
            else if (this._labelState == LabelState.Hidden)
            {
                this.FadeIn(milliseconds);
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}