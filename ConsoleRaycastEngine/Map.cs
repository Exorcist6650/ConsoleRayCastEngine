using ConsoleReycastEngine.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReycastEngine
{
    public struct Map(int[,] field)
    {
        // Matrix
        public int[,] Field { get; set; } = field;

        // Parameters
        public readonly int Width { get; init; } = field.GetLength(0);
        public readonly int Height { get; init; } = field.GetLength(1);


        public bool IsObstacle(Vector2 location)
        {
            int x = (int)Math.Round(location.X);
            int y = (int)Math.Round(location.Y);

            return x < 0 || x >= Width || 
                y < 0 || y >= Height || 
                Field[y, x] == 1;
        }
    }
}
