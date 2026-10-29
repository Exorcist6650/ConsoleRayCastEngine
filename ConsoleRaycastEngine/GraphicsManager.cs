using ConsoleReycastEngine.core;
using System.Numerics;

namespace ConsoleReycastEngine
{
    public class GraphicsManager
    {
        // Dependencies
        private readonly Map _map;
        private readonly Player _player;

        // States
        public int ScreenHeight { get; init; } = Console.BufferHeight;
        public int ScreenWidth { get; init; } = Console.BufferWidth;

        private char[,] _previousBuffer;
        private char[,] _currentBuffer;
        
        private bool _isInit = false;


        public GraphicsManager(
            Map map, 
            Player player)
        {
            // Checking for empty field
            if (map.Height == 0 || map.Width == 0)
                throw new ArgumentException("Field is empty");

            _map = map;
            _player = player;
        }

        // METHODS


        public void Init()
        {
            // Buffers initialization
            _previousBuffer = new char[ScreenHeight, ScreenWidth];
            _currentBuffer = new char[ScreenHeight, ScreenWidth];

            // Empty characters setting
            for (int i = 0; i < ScreenHeight; i++)
                for (int j = 0; j < ScreenWidth; j++)
                {
                    _previousBuffer[i, j] = ' ';
                    _currentBuffer[i, j] = '.';
                }

            // Hide cursor
            Console.CursorVisible = false; 

            // Switch status
            _isInit = true;
        }


        // Displaying 3D 
        public void Add3dToBuffer(params IList<(int distance, float angle)> rays)
        {
            if (!_isInit)
                throw new InvalidOperationException("Manager is not init");

            var screenSegmentsAmount = rays.Count;
            var segmentWidth = ScreenWidth / screenSegmentsAmount;

            for (int i = 0; i < rays.Count; i++)
            {
                var ray = rays[i];

                if (ray.distance == -1) continue;

                // Ray rotation to radians 
                var rayRotationRadians = Tools.ToRadians((ray.angle));
                
                // Distance with fixing fish-eye effect
                float currentDistance = (float)
                    (ray.distance * Math.Cos(rayRotationRadians));

                char wallSymbol = (int)Math.Round(currentDistance) switch
                {
                    0 => '█',
                    1 => '█',
                    2 => '▓',
                    3 => '▓',
                    4 => '▒',
                    5 => '▒',
                    _ => '░',
                };

                // Height in float
                float targetHeight = ScreenHeight / (currentDistance + 1f);

                // Smooth height
                float wallHeight = targetHeight;
                int wallWidth = segmentWidth;

                int widthCenter = (i * segmentWidth) + segmentWidth / 2;
                int heightCenter = ScreenHeight / 2;

                float halfHeight = wallHeight / 2f;
                int halfWidth = wallWidth / 2;


                int yStart = (int)Math.Floor(heightCenter - halfHeight);
                int yEnd = (int)Math.Ceiling(heightCenter + halfHeight);
                int xStart = Math.Max(0, widthCenter - halfWidth);
                int xEnd = Math.Min(ScreenWidth - 1, widthCenter + halfWidth);

                for (int y = yStart; y < yEnd; y++)
                {
                    for (int x = xStart; x < xEnd; x++)
                    {
                        _currentBuffer[y, x] = wallSymbol;
                    }
                }
            }
        }


        // Displaying map on 2D
        public void Add2dMapToBuffer() 
        {
            if (!_isInit) 
                throw new InvalidOperationException("Manager is not init");

            if (_map.Height > ScreenHeight || _map.Width > ScreenWidth)
                throw new ArgumentException("Map is bigger than screen");

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


            // Add map to console buffer
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    // Map symbol from int
                    char mapSymbol = matrix[y, x] switch
                    {
                        0 => '`',
                        1 => '=',
                        2 => playerSymbol,
                        _ => '?'
                    };

                    _currentBuffer[y, x] = mapSymbol;
                }
            }
        }


        public void AddDebugInfoToBuffer(
            int x, int y, params string[] textLines)
        {
            // Text line
            for (int l = 0; l < textLines.Length; l++)
            {
                string currentLine = textLines[l];

                // Line letter
                for (int c = 0; c < textLines[l].Length; c++)
                {
                    char currentLetter = currentLine[c];
                    int letterPositionX = x + c;
                    int letterPositionY = y + l;

                    // Checking screen sizes
                    if (letterPositionX >= 0 && letterPositionX < ScreenWidth && 
                        letterPositionY >= 0 && letterPositionY < ScreenHeight)
                    {
                        // Add letters to buffer
                        _currentBuffer[letterPositionY, letterPositionX] 
                            = currentLetter;
                    }
                }
            }
        }


        public void ClearBuffer()
        {
            for (int i = 0; i < ScreenHeight; i++)
                for (int j = 0; j < ScreenWidth; j++)
                    _currentBuffer[i, j] = '.';
        }


        public void DisplayBuffer()
        {
            for (int y = 0; y < ScreenHeight; y++)
            {
                for (int x = 0; x < ScreenWidth; x++)
                {
                    // Update only differents elements
                    if (_previousBuffer[y, x] != _currentBuffer[y, x])
                    {
                        // Move cursor and write
                        Console.SetCursorPosition(x, y);
                        Console.Write(_currentBuffer[y, x]);

                        // Update previous buffer
                        _previousBuffer[y, x] = _currentBuffer[y, x];
                    }
                }
            }
        }
    }
}
