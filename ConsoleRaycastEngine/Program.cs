using ConsoleReycastEngine.core;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ConsoleReycastEngine
{
    class Program 
    {
        // Win api key input
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        // Keyboard input
        static bool IsPressed(int key) =>
            (GetAsyncKeyState(key) & 0x8000) != 0;


        // Add logic by input
        public static void InputHandler(
            Player player, 
            Map map, 
            float deltaSeconds = 1)
        {
            // Checking keys
            bool w_key = IsPressed(0x57);
            bool s_key = IsPressed(0x53);
            bool a_key = IsPressed(0x41);
            bool d_key = IsPressed(0x44);
            bool left_key = IsPressed(0x25);
            bool right_key = IsPressed(0x27);
            bool escape_key = IsPressed(0x1B);


            // Add movement
            if (w_key) player.Move(map, Direction.Forward, deltaSeconds);
            if (s_key) player.Move(map, Direction.Backward, deltaSeconds);
            if (a_key) player.Move(map, Direction.Left, deltaSeconds);
            if (d_key) player.Move(map, Direction.Right, deltaSeconds);

            // Camera rotation
            if (left_key)
                player.Look(CameraDirection.Left, deltaSeconds);
            if (right_key)
                player.Look(CameraDirection.Right, deltaSeconds);

            // Menu 
            if (escape_key) Environment.Exit(0);
        }

        public static void Main()
        {
            int[,] field = { 
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            // INITIALIZATION OBJECTS

            // Game map
            var map = new Map(field);

            // Base states
            var playerLocation = new Vector2(map.Width / 2, map.Height / 2);

            // Camera
            var cameraComponent = new CameraComponent(
                    location: playerLocation,
                    rotation: 0,
                    rotationSpeed: 90,
                    viewDistance: 3);

            // Player
            var player = new Player(
                cameraComponent,
                location: playerLocation,
                speed: 3f);

            // Manager init
            var graphicsManager = new GraphicsManager(map, player);

            // Screen parameters
            const int TargetFps = 60;
            const int FrameTimeMs = 1000 / TargetFps;

            Console.CursorVisible = false; // Hide cursor

            DateTime lastTime = new();
            // Main loop
            while (true)
            {
                // Delta time 
                var currentTime = DateTime.UtcNow;
                var deltaTime = currentTime - lastTime;
                lastTime = currentTime;

                float deltaSeconds = (float)deltaTime.TotalSeconds;

                // Buttons handler
                InputHandler(player, map, deltaSeconds);

                // Map display
                graphicsManager.ClearDisplay();
                graphicsManager.Display2D();

                // DEBUG INFO
                Console.WriteLine($"View angle: {player.CameraComponent.ViewAngle}");

                Console.WriteLine($"Ray X | Y: {player.CameraComponent.RayLocation.X} | " +
                                  $"{player.CameraComponent.RayLocation.Y}");

                Console.WriteLine($"FPS: {1000 / deltaTime.Milliseconds}");


                Thread.Sleep(1); // Frame waiting
            }

        }
    }
}