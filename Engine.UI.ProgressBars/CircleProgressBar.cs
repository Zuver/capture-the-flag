using Engine.Drawing;
using Microsoft.Xna.Framework;
using Engine.Utilities;

namespace Engine.UI.ProgressBars
{
    public class CircleProgressBar : AbstractProgressBar
    {
        public CircleProgressBar(Color color)
        {
            Primitive = PrimitiveFactory.Circle(color, AppSettingsFacade.PlayerRadius, 100);
        }

        public void SetProgress(float progress) => Primitive.SetFillPercentage(progress);
    }

    public enum CircleProgressBarOrigin
    {
        Top,
        Right,
        Down,
        Bottom
    };

    public enum CircleProgreeBarDecayDirection
    {
        Clockwise,
        CounterClockwise
    };
}
