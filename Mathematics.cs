using OpenTK.Mathematics;

namespace TheSolarSystem
{
    public static class Mathematics
    {
        public static Matrix4 Rotation(float angle, float x, float y, float z)
        {
            Matrix4 rotation = new Matrix4()
            {
                Column0 = new Vector4(1, 0, 0, 0),
                Column1 = new Vector4(0, 1, 0, 0),
                Column2 = new Vector4(0, 0, 1, 0),
                Column3 = new Vector4(0, 0, 0, 1)
            };
            if (x == 1)
            {
                Matrix4 matrix = new Matrix4();
                Matrix4.CreateRotationX(angle, out matrix);
                rotation = rotation * matrix;
            }
            if (y == 1)
            {
                Matrix4 matrix = new Matrix4();
                Matrix4.CreateRotationY(angle, out matrix);
                rotation = rotation * matrix;
            }
            if (z == 1)
            {
                Matrix4 matrix = new Matrix4();
                Matrix4.CreateRotationZ(angle, out matrix);
                rotation = rotation * matrix;
            }
            return rotation;
        }

    }
}
