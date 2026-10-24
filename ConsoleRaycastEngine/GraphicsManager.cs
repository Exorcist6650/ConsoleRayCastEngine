using System.Numerics;

namespace ConsoleReycastEngine
{
    public class GraphicsManager
    {
        // States
        private readonly Map _map;
        private readonly Player _player;

        public GraphicsManager(Map map, Player player)
        {
            // Checking for empty field
            if (map.Height == 0 && map.Width == 0) 
                throw new ArgumentNullException("Field is empty");

            _map = map;
            _player = player;
        }


        // Displaying map on 2D
        public void Display2D() 
        {
            var matrix = (int[,])_map.Field.Clone();

            // Add player to field
            char playerSymbol = _player.CameraComponent.ViewAngle switch
            {
                >= 337.5f or < 22.5f => '-',
                >= 22.5f and < 67.5f => '\\',
                >= 67.5f and < 112.5f => '|',
                >= 112.5f and < 157.5f => '/',
                >= 157.5f and < 202.5f => '-',
                >= 202.5f and < 247.5f => '\\',
                >= 247.5f and < 292.5f => '|',
                >= 292.5f and < 337.5f => '/',
                _ => 'X'
            };

            // Add player to matrix
            matrix[(int)Math.Round(_player.Location.Y), 
                (int)Math.Round(_player.Location.X)] = 2; 


            // Display game field
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    // Map symbol from int
                    char mapSymbol = matrix[y, x] switch
                    {
                        0 => '.',
                        1 => '=',
                        2 => playerSymbol,
                        _ => '?'
                    };

                    Console.Write(mapSymbol); // Symbols displaying
                    Console.Write(' ');
                }
                Console.WriteLine(); // New line
            }
        }

        public void ClearDisplay() =>
            Console.Clear();
    }
}
