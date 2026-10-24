using ConsoleReycastEngine.core;
using System.Numerics;

namespace ConsoleReycastEngine
{
    public enum CameraDirection : byte
    {
        Left,
        Right
    }


    public class CameraComponent
    {
        public CameraComponent(
            Vector2 location,
            int rotation,
            int rotationSpeed,
            float viewDistance)
        {
            Location = location;

            ViewAngle = Tools.NormalizeAngle(rotation);
            RotationSpeed = rotationSpeed;
            ViewDistance = viewDistance;

            CalculateRayLocation(); // View ray location
        }

        // Props
        public Vector2 Location { get; set; }

        public float ViewAngle { get; set; }

        public float RotationSpeed { get; set; }

        public float ViewDistance { get; } 

        public Vector2 RayLocation { get; private set; }



        // PUBLIC 
        public void Rotate(
            CameraDirection cameraDirection, 
            float DeltaSeconds = 1)
        {
            switch (cameraDirection)
            {
                case CameraDirection.Left:
                    ViewAngle = Tools.NormalizeAngle(
                        ViewAngle - RotationSpeed * DeltaSeconds);
                    break;

                case CameraDirection.Right:
                    ViewAngle = Tools.NormalizeAngle(
                        ViewAngle + RotationSpeed * DeltaSeconds);
                    break;
            }
        }

        public void CalculateRayLocation() =>
            RayLocation = Tools.GetVectorEndLocation(
                Location, ViewAngle, ViewDistance);
    }
}
