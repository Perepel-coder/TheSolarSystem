using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace TheSolarSystem
{
    public class ShaderProgram
    {
        private readonly int vertexShaderID;
        private readonly int fragmentShaderID;
        private readonly int programShaderID;


        public ShaderProgram(string vertexFile, string fragmentFile)
        {
            vertexShaderID = CreatShader(ShaderType.VertexShader, vertexFile);
            fragmentShaderID = CreatShader(ShaderType.FragmentShader, fragmentFile);

            programShaderID = GL.CreateProgram();
            GL.AttachShader(programShaderID, vertexShaderID);
            GL.AttachShader(programShaderID, fragmentShaderID);
            GL.LinkProgram(programShaderID);

            GL.GetProgram(programShaderID, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(programShaderID);
                throw new Exception($"Ошибка при компиляции программы шейдера № {programShaderID} \n\n {infoLog}");
            }

            DeleteShader(vertexShaderID);
            DeleteShader(fragmentShaderID);
        }

        public void ActiveProgram() => GL.UseProgram(programShaderID);

        public void DeactiveProgram() => GL.UseProgram(0);

        public void DeleteProgram() => GL.DeleteProgram(programShaderID);

        public int GetAttribProgram(string name) => GL.GetAttribLocation(programShaderID, name);

        public void SetUniform4(string name, Vector4 vec)
        {
            int location = GL.GetUniformLocation(programShaderID, name);
            GL.Uniform4(location, vec);
        }
        public void SetUniform1(string name, int vec)
        {
            int location = GL.GetUniformLocation(programShaderID, name);
            GL.Uniform1(location, vec);
        }
        public void SetUniformMatrix4(string name, Matrix4 mat)
        {
            int location = GL.GetUniformLocation(programShaderID, name);
            GL.UniformMatrix4(location, false, ref mat);
        }
        private int CreatShader(ShaderType shaderType, string shaderFile)
        {
            string shaderSTR = File.ReadAllText(shaderFile);
            int shaderID = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderID, shaderSTR);
            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out var code);
            if(code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shaderID);
                throw new Exception($"Ошибка при компиляции шейдера № {shaderID} \n\n {infoLog}");
            }

            return shaderID;
        }

        private void DeleteShader(int shader)
        {
            GL.DetachShader(programShaderID, shader);
            GL.DeleteShader(shader);
        }
    }
}
