using OpenTK.Mathematics;

namespace TheSolarSystem
{
    public static class Camera
    {
        // скорость движения камеры + скорость движения мыши
        public static float Speed { get; set; }
        public static float MouseSensitivity { get; set; }

        // текущая позиция камеры
        public static Vector3 Position { get; set; }

        // ориентация камеры камеры
        public static Vector3 Orientation { get; set; }

        // матрица вида
        public static Matrix4 View { get; set; }

        public static void InitCamera(float _speed, float _mouseSensitivity, Vector3 _position, Vector3 _orientation)
        {
            Speed = _speed;
            MouseSensitivity = _mouseSensitivity;
            Position = _position;
            Orientation = _orientation;
            GetViewMatrix();
        }

        public static void GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            View = Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public static void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, Speed);

            Position += offset;
        }
        public static void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            float X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            float Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
            Orientation = new Vector3(X, Y, Orientation.Z);
        }
    }
}
