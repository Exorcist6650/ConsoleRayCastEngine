using System.Numerics;

namespace ConsoleReycastEngine
{
    public class GraphicsManager
    {
        // Dependencies
        private readonly Map _map;
        private readonly Player _player;

        // States
        public int ScreenHeight { get; init; }
        public int ScreenWidth { get; init; }

        private char[,] _previousBuffer;
        private char[,] _currentBuffer;

        private bool _isInit = false;


        public GraphicsManager(
            Map map, 
            Player player, 
            int screenHeight, 
            int screenWidth)
        {
            // Checking for empty field
            if (map.Height == 0 && map.Width == 0) 
                throw new ArgumentNullException("Field is empty");

            if (screenHeight <= 0 && screenWidth <= 0)
                throw new ArgumentNullException("Invalid screen format");

            _map = map;
            _player = player;

            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;

            
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
                    _currentBuffer[i, j] = ' ';
                }

            try
            {
                // Set size to screen
                Console.WindowHeight = ScreenHeight;
                Console.WindowWidth = ScreenWidth;

                Console.CursorVisible = false; // Hide cursor
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // Switch status
            _isInit = true;
        }
        // Displaying map on 2D
        public void Add2dMapToBuffer() 
        {
            if (!_isInit) 
                throw new InvalidOperationException("Manager is not init");

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
                        0 => '.',
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

                    // Checking screen sizes
                    if (x >= 0 && x < ScreenWidth && y >= 0 && y < ScreenWidth)
                    {
                        // Add letters to buffer
                        _currentBuffer[y + l, x + c] = currentLetter;
                    }
                }

                // Clear previous symbols
                for (int lineOffset = currentLine.Length; 
                    lineOffset < ScreenWidth; 
                    lineOffset++)
                {
                    // Checking screen sizes
                    if (x >= 0 && x < ScreenWidth && y >= 0 && y < ScreenWidth)
                    {
                        // Empty symbols
                        _currentBuffer[y + l, x + lineOffset] = ' ';
                    }
                }
            }
        }

        public void DisplayBuffer()
        {
            for (int y = 0; y < ScreenHeight; y++)
            {
                for (int x = 0; x < ScreenHeight; x++)
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
