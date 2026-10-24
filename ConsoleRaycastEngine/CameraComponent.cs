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
        }

        // Props
        public Vector2 Location { get; set; }

        public float ViewAngle { get; set; }

        public float RotationSpeed { get; set; }

        public float ViewDistance { get; } 

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
        
        /// <summary>
        /// Casts ray and try to find distance to obstacle
        /// </summary>
        /// <param name="map"></param>
        /// <param name="rayAngle"></param>
        /// <param name="rayLength"></param>
        /// <returns>Distance to obstacle. Otherwise -1</returns>
    }
}
