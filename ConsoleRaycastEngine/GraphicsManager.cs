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
            matrix[(int)Math.Round(_player.Location.Y), 
                (int)Math.Round(_player.Location.X)] = 2; 

            // Draw view ray
            if (!_map.IsObstacle(_player.CameraComponent.RayLocation))
                matrix[(int)Math.Round(_player.CameraComponent.RayLocation.Y), 
                    (int)Math.Round(_player.CameraComponent.RayLocation.X)] = 2;

            // Display game field
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    Console.Write(matrix[i, j]); // Symbols displaying
                    Console.Write(' ');
                }
                Console.WriteLine(); // New line
            }
        }

        public void ClearDisplay() =>
            Console.Clear();
    }
}
