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
            int[,] field =
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
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
                    viewDistance: 20,
                    fieldOfView: 90);

            // Player
            var player = new Player(
                cameraComponent,
                location: playerLocation,
                speed: 3f);

            // Manager init
            var graphicsManager = new GraphicsManager(map, player);
            graphicsManager.Init();

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


                // CALCULATE RAYS

                // Rays that camera spawns
                int raysAmount = 60; 

                // Step between rays
                float rayStep = raysAmount > 1 
                    ? player.CameraComponent.FOV / (raysAmount - 1) 
                    : 0f;

                // Offset to start
                float startAngleOffset = raysAmount > 1
                    ? -(player.CameraComponent.FOV / 2f)
                    : 0f;

                // Distance results of all rays
                List<(int distance, float angle)> raysDistances = new();

                // Calculate distance to all rays
                for (int i = 0; i < raysAmount; i++)
                {
                    // Ray rotation include camera rotation
                    float currentRayAngle = Tools.NormalizeAngle(
                        player.CameraComponent.ViewAngle +
                        (startAngleOffset + (rayStep * i)));

                    // Ray distance to obstacle
                    var currentRayDistance = Tools.TryGetDistanceToObstacle(
                        map,
                        player.CameraComponent.Location,
                        currentRayAngle,
                        player.CameraComponent.ViewDistance);

                    var deltaRayAngle = player.CameraComponent.ViewAngle - currentRayAngle;
                    raysDistances.Add((currentRayDistance, deltaRayAngle));
                }

                // RENDER


                // Add debug text to render buffer
                //graphicsManager.AddDebugInfoToBuffer(0, map.Height,
                //    $"FPS: {1000 / deltaTime.Milliseconds}",
                //    $"View angle: {player.CameraComponent.ViewAngle}",
                //    $"Distance to obstacle front: {distanceForward}");

                graphicsManager.ClearBuffer(); // Clear

                // Add camera view in 3d
                graphicsManager.Add3dToBuffer(raysDistances);

                // Add 2d to render buffer
                graphicsManager.Add2dMapToBuffer();

                graphicsManager.DisplayBuffer(); // Draw

                Thread.Sleep(1); // Frame waiting
            }
        }
    }
}