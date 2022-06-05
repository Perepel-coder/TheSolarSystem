using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace TheSolarSystem
{
    public enum BufferType
    {
        ArrayBuffer = BufferTarget.ArrayBuffer,
        ElementBuffer = BufferTarget.ElementArrayBuffer
    }

    public enum BufferHint
    {
        StaticDraw = BufferUsageHint.StaticDraw,
        DynamicDraw = BufferUsageHint.DynamicDraw
    }

    // VBO - буфер, находящийся в памяти видеокарты
    public sealed class BufferObject : IDisposable
    {
        // Id нового буффера
        public int BufferID { get; private set; }

        // тип данного буффера
        private readonly BufferTarget type;

        // флаг активности
        private bool active;

        public BufferObject(BufferType type)
        {
            this.type = (BufferTarget)type;
            BufferID = GL.GenBuffer();
            this.active = false;
        }

        // заполнить буфер данными
        // принимает: тип буффера, размер буффера, массив данных, тип доступа к данным
        public void SetData<T>(T[] data, BufferHint hint) where T:struct
        {
            if(data.Length == 0) { throw new Exception("данные не найдены"); }
            Activate();
            GL.BufferData(type, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, (BufferUsageHint)hint);
        }

        // активация буфера
        public void Activate()
        {
            active = true;
            GL.BindBuffer(type, BufferID);
        }

        // деактивация буффера
        public void Deactivate()
        {
            active = false;
            GL.BindBuffer(type, 0);
        }

        // активен ли буффер?
        public bool IsActivate() 
        {
            return active;
        }

        // удалить буффер
        private void Delete()
        {
            if(BufferID == -1)
            {
                return;
            }
            Deactivate();
            GL.DeleteBuffer(BufferID);

            BufferID = -1;
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
            throw new NotImplementedException();
        }
    }
}
