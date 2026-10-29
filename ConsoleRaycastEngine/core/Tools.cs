using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReycastEngine.core
{
    public static class Tools
    {
        public static int TryGetDistanceToObstacle(Map map, Vector2 rayStart, float rayAngle, float rayLength)
        {
            for (int currentDistance = 1;
                currentDistance <= (int)Math.Round(rayLength);
                currentDistance++)
            {
                var rayLocation =
                    GetVectorEndLocation(rayStart, rayAngle, currentDistance);

                // Return distance to obstacle
                if (map.IsObstacle(rayLocation))
                    return currentDistance - 1;
            }
            return -1; // Not obstacle
        }

        public static Vector2 GetVectorEndLocation(
            Vector2 startPosition,
            float angle,
            float length)
        {
            var viewRadians = angle * Math.PI / 180.0f;

            // End view location
            float endX = (float)
                (startPosition.X + length * Math.Cos(viewRadians));

            float endY = (float)
                (startPosition.Y + length * Math.Sin(viewRadians));

            return new Vector2(endX, endY);
        }

        public static float NormalizeAngle(float angle) =>
            (angle % 360 + 360) % 360;

        public static float ToRadians(float angle) =>
            (float)(angle * Math.PI / 180.0f);


        public static int GetAngleByDirection(Direction direction) => direction switch
        {
            Direction.Forward => 0,
            Direction.Backward => 180,
            Direction.Left => 270,
            Direction.Right => 90,
            _ => throw new ArgumentException("Not valid direction")
        };

    }
}
