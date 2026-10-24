using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReycastEngine.core
{
    public static class Tools
    {
        public static Vector2 GetVectorEndLocation(
            Vector2 startPosition,
            float rotation,
            float length)
        {
            var viewRadians = rotation * Math.PI / 180.0f;

            // End view location
            float endX = (float)
                (startPosition.X + length * Math.Cos(viewRadians));

            float endY = (float)
                (startPosition.Y + length * Math.Sin(viewRadians));

            return new Vector2(endX, endY);
        }

        public static float NormalizeAngle(float angle) =>
            (angle % 360 + 360) % 360;

        public static int GetAngleByDirection(Direction direction) => direction switch
        {
            Direction.Forward => 0,
            Direction.Backward => 180,
            Direction.Left => -90,
            Direction.Right => 90,
            _ => throw new ArgumentException("Not valid direction")
        };

    }
}
