using Engine.Drawing;
using Microsoft.Xna.Framework;
using Engine.Utilities;

namespace Engine.UI.ProgressBars
{
    public class CircleProgressBar : AbstractProgressBar
    {
        public CircleProgressBar(Color color)
        {
            this.Primitive = PrimitiveFactory.Circle(color, AppSettingsFacade.PlayerRadius, 100);
        }
    }
}