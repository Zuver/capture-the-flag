using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Bodies.Tests
{
    public class LineBodyTests
    {
        private const float EPSILON = 0.00001f;

        [Test]
        public void VerticalLine()
        {
            Vector2 start = Vector2.Zero;
            Vector2 end = new Vector2(0f, 5f);
            LineBody lineBody = BodyFactory.AdHocLine(start, end);

            Vector2 pointInQuestion = new Vector2(0f, 10f);
            Vector2 result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(0f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(5f).Within(EPSILON));
        }

        [Test]
        public void HorizontalLine()
        {
            Vector2 start = Vector2.Zero;
            Vector2 end = new Vector2(5f, 0f);
            LineBody lineBody = BodyFactory.AdHocLine(start, end);

            Vector2 pointInQuestion = new Vector2(10f, 0f);
            Vector2 result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(5f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(0f).Within(EPSILON));
        }

        [Test]
        public void PositiveSlopeLine()
        {
            Vector2 start = new Vector2(-5f, -5f);
            Vector2 end = new Vector2(5f, 5f);
            LineBody lineBody = BodyFactory.AdHocLine(start, end);

            Vector2 pointInQuestion = new Vector2(4f, 2f);
            Vector2 result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(3f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(3f).Within(EPSILON));

            pointInQuestion = new Vector2(40f, 20f);
            result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(5f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(5f).Within(EPSILON));

            pointInQuestion = new Vector2(-40f, -20f);
            result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(-5f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(-5f).Within(EPSILON));
        }

        [Test]
        public void NegativeSlopeLine()
        {
            Vector2 start = Vector2.Zero;
            Vector2 end = new Vector2(5f, -5f);
            LineBody lineBody = BodyFactory.AdHocLine(start, end);

            Vector2 pointInQuestion = new Vector2(4f, -2f);
            Vector2 result = lineBody.GetClosestPointOnPerimeter(pointInQuestion);

            Assert.That(result.X, Is.EqualTo(3f).Within(EPSILON));
            Assert.That(result.Y, Is.EqualTo(-3f).Within(EPSILON));
        }
    }
}
