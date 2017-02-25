using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Drawing
{
    public static class PrimitiveFactory
    {
        /// <summary>
        /// Creates a new PrimitiveCircle object
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="radius">Color</param>
        /// <param name="numVertices">Color</param>
        /// <returns>PrimitiveCircle reference</returns>
        public static CirclePrimitive Circle(Color color, float radius, int numVertices)
        {
            return new CirclePrimitive(color, radius, numVertices);
        }

        /// <summary>
        /// Creates a new PrimitiveLine object
        /// </summary>
        /// <param name="color"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static LinePrimitive Line(Color color, Vector2 start, Vector2 end)
        {
            return new LinePrimitive(color, start, end);
        }

        /// <summary>
        /// Creates a new DottedLinePrimitive object
        /// </summary>
        /// <param name="color"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="lineLength"></param>
        /// <param name="lineSpacing"></param>
        /// <returns></returns>
        public static DottedLinePrimitive DottedLine(Color color, Vector2 startPosition, Vector2 endPosition, float lineLength, float lineSpacing)
        {
            return new DottedLinePrimitive(color, startPosition, endPosition, lineLength, lineSpacing);
        }

        /// <summary>
        /// Creates a new PrimitiveRectangle object
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectanglePrimitive Rectangle(Color color, int width, int height)
        {
            return new RectanglePrimitive(width, height, color);
        }
    }
}