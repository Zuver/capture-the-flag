using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class ExtensionMethods
    {
        public static bool HasSameDirection(this Vector2 source, Vector2 vector)
        {
            float dotProduct = Vector2.Dot(source, vector);
            return Math.Abs(dotProduct - source.Length() * vector.Length()) < 0.1f;
        }
    }
}