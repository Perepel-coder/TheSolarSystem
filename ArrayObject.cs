using OpenTK.Graphics.OpenGL;


namespace TheSolarSystem
{

    public enum AttribType
    {
        Float = VertexAttribPointerType.Float
    }

    public enum ElementType
    {
        UnsignedInt = DrawElementsType.UnsignedInt
    }

    // массив, содержащий VBO, а также последовательность индексов вершин (порядок отрисовки)
    public class ArrayObject : IDisposable
    {
        public int ArrayID { get; private set; }

        private bool active = false;

        private List<int> attribsList;
        private List<BufferObject> buffersList;

        public ArrayObject()
        {
            attribsList = new List<int>();
            buffersList = new List<BufferObject>();
            ArrayID = GL.GenVertexArray();
        }

        public void Activate()
        {
            active = true;
            GL.BindVertexArray(ArrayID);
        }

        public void Deactivate()
        {
            active = false;
            GL.BindVertexArray(0);
        }

        public bool IsActive()
        {
            return active;
        }

        // Привяязывает буффер к текущему VAO
        public void AttachBuffer(BufferObject buffer)
        {
            if (IsActive() != true)
            {
                Activate();
            }
            buffer.Activate();
            buffersList.Add(buffer);
        }

        // активирует нужный атрибут
        public void AttribPointer(int index, int elementsPerVertex, AttribType type, bool norm, int stride, int offset)
        {
            attribsList.Add(index);

            // выделяем память под буффер (атрибут массива)
            GL.EnableVertexAttribArray(index);

            // задаем правила чтения
            GL.VertexAttribPointer(
                index, elementsPerVertex, 
                (VertexAttribPointerType)type, 
                norm, stride, offset);
        }

        public void Draw(PrimitiveType pt, int start, int count)
        {
            Activate();
            GL.DrawArrays(pt, start, count);
        }

        public void DrawElements(PrimitiveType pt, int start, int count, ElementType type)
        {
            Activate();
            GL.DrawElements(pt, count, (DrawElementsType)type, start);
        }

        public void DisableAttribAll()
        {
            foreach (int attrib in attribsList)
                GL.DisableVertexAttribArray(attrib);
        }

        private void Delete()
        {
            if (ArrayID == -1)
                return;

            Deactivate();
            GL.DeleteVertexArray(ArrayID);

            foreach (BufferObject buffer in buffersList)
                buffer.Dispose();

            ArrayID = -1;
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
