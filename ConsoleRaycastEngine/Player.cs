using ConsoleReycastEngine.core;
using System;
using System.Numerics;

namespace ConsoleReycastEngine
{
    public enum Direction : byte
    {
        Forward,
        Backward,
        Left,
        Right,
    }

    public class Player(
        CameraComponent cameraComponent,
        Vector2 location,
        float speed)
    {
        // Dependencies
        public CameraComponent CameraComponent { get; init; }
            = cameraComponent;

        // States 
        public Vector2 Location { get; set; } = location;

        public float Speed { get; set; } = speed;


        public bool Move(
            Map map, 
            Direction direction, 
            float deltaSeconds = 1)
        {
            // Move angle from current view angle
            var moveAngle = CameraComponent.ViewAngle + 
                Tools.GetAngleByDirection(direction);

            // New location with view angle
            var newLocation = Tools.GetVectorEndLocation(
                Location,
                Tools.NormalizeAngle(moveAngle),
                length: Speed * deltaSeconds);

            // Checking obstacle
            if (map.IsObstacle(newLocation))
                return false;

            Location = newLocation; // Update self location
            CameraComponent.Location = newLocation; // Update camera location
            CameraComponent.CalculateRayLocation(); // Update view ray

            return true; // If player was moved
        }


        public void Look(
            CameraDirection direction, 
            float deltaSeconds = 1)
        {
            // Rotate camera
            CameraComponent.Rotate(direction, deltaSeconds); 

            // Update view ray
            CameraComponent.CalculateRayLocation(); 
        }
    }
}
