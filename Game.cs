using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace TheSolarSystem
{
    public class Game: GameWindow
    {
        #region поля
        private bool drawTrajectoryFLAG;
        private bool drawSystemEarthMoonFLAG;
        private bool drawPolygonFLAG;
        private Vector2 lastMousePos;

        private ShaderProgram shaderProgramBS;
        private ShaderProgram shaderProgramTS;
        private ShaderProgram shaderProgramSBS;

        private readonly int[] texturePlanet;

        List<float> skyboxVertices = new List<float>
        {
            #region skyboxVertices
             0.0f, 1.0f, 0.0f,      1.0f, 0.0f,
             0.0f, 0.0f, 0.0f,      0.0f, 0.0f,
             1.0f, 0.0f, 0.0f,      0.0f, 1.0f,
             1.0f, 0.0f, 0.0f,      0.0f, 1.0f,
             1.0f, 1.0f, 0.0f,      1.0f, 1.0f,
             0.0f, 1.0f, 0.0f,      1.0f, 0.0f,

             0.0f, 0.0f, 1.0f,      1.0f, 0.0f,
             0.0f, 0.0f, 0.0f,      0.0f, 0.0f,
             0.0f, 1.0f, 0.0f,      0.0f, 1.0f,
             0.0f, 1.0f, 0.0f,      0.0f, 1.0f,
             0.0f, 1.0f, 1.0f,      1.0f, 1.0f,
             0.0f, 0.0f, 1.0f,      1.0f, 0.0f,

             1.0f, 0.0f, 0.0f,      1.0f, 0.0f,     
             1.0f, 0.0f, 1.0f,      0.0f, 0.0f,
             1.0f, 1.0f, 1.0f,      0.0f, 1.0f,
             1.0f, 1.0f, 1.0f,      0.0f, 1.0f,
             1.0f, 1.0f, 0.0f,      1.0f, 1.0f,
             1.0f, 0.0f, 0.0f,      1.0f, 0.0f,

             0.0f, 0.0f,  1.0f,     1.0f, 0.0f,     
             0.0f, 1.0f,  1.0f,     0.0f, 0.0f,
             1.0f, 1.0f,  1.0f,     0.0f, 1.0f,
             1.0f, 1.0f,  1.0f,     0.0f, 1.0f,
             1.0f, 0.0f,  1.0f,     1.0f, 1.0f,
             0.0f, 0.0f,  1.0f,     1.0f, 0.0f,

             0.0f, 1.0f, 0.0f,      1.0f, 0.0f,     
             1.0f, 1.0f, 0.0f,      0.0f, 0.0f,
             1.0f, 1.0f, 1.0f,      0.0f, 1.0f,
             1.0f, 1.0f, 1.0f,      0.0f, 1.0f,
             0.0f, 1.0f, 1.0f,      1.0f, 1.0f,
             0.0f, 1.0f, 0.0f,      1.0f, 0.0f,

             0.0f, 0.0f, 0.0f,      1.0f, 0.0f,     
             0.0f, 0.0f, 1.0f,      0.0f, 0.0f,
             1.0f, 0.0f, 0.0f,      0.0f, 1.0f,
             1.0f, 0.0f, 0.0f,      0.0f, 1.0f,
             0.0f, 0.0f, 1.0f,      1.0f, 1.0f,
             1.0f, 0.0f, 1.0f,      1.0f, 0.0f
            #endregion
        };

        #region экзэмпляры планет + vao планет

        private CreatPlanet sun;
        private ArrayObject sunVAO;

        private CreatPlanet mercury;
        private ArrayObject mercuryVAO;
        private ArrayObject mercuryTrajectoryVAO;

        private CreatPlanet venus;
        private ArrayObject venusVAO;
        private ArrayObject venusTrajectoryVAO;

        private CreatPlanet earth;
        private ArrayObject earthVAO;
        private ArrayObject earthTrajectoryVAO;

        private CreatPlanet mars;
        private ArrayObject marsVAO;
        private ArrayObject marsTrajectoryVAO;

        private CreatPlanet jupiter;
        private ArrayObject jupiterVAO;
        private ArrayObject jupiterTrajectoryVAO;

        private CreatPlanet saturn;
        private ArrayObject saturnVAO;
        private ArrayObject saturnTrajectoryVAO;

        private CreatPlanet uranus;
        private ArrayObject uranusVAO;
        private ArrayObject uranusTrajectoryVAO;

        private CreatPlanet neptune;
        private ArrayObject neptuneVAO;
        private ArrayObject neptuneTrajectoryVAO;

        private CreatPlanet moon;
        private ArrayObject moonVAO;
        private ArrayObject moonTrajectoryVAO;

        private ArrayObject skyboxVAO;

        #endregion

        private float rotationSpeed;
        private float speedMovement;

        #endregion

        #region конструкторы
        public Game(
            GameWindowSettings gameWindowSettings, 
            NativeWindowSettings nativeWindowSettings
            ) : base( gameWindowSettings, nativeWindowSettings) 
        {
            drawTrajectoryFLAG = true;
            drawSystemEarthMoonFLAG = false;
            drawPolygonFLAG = true;
            lastMousePos = new Vector2();

            // активация шейдерной программы
            shaderProgramBS = new ShaderProgram(
                @"..\..\..\Data\Shaders\BaseShader.vert",
                @"..\..\..\Data\Shaders\BaseShader.frag");

            shaderProgramTS = new ShaderProgram(
                @"..\..\..\Data\Shaders\TrajectoryShader.vert",
                @"..\..\..\Data\Shaders\TrajectoryShader.frag");

            shaderProgramSBS = new ShaderProgram(
                @"..\..\..\Data\Shaders\SkyboxShader.vert",
                @"..\..\..\Data\Shaders\SkyboxShader.frag");

            // инициализация массива текстур
            texturePlanet = new int[11];

            // инициализация планет
            #region инициализация планет

            sun = new CreatPlanet(16.0f, 30, 30, 0.0f, 0.0018027f, 0, new Vector4(0, 0, 0, 1), new Vector3 (0, 0, 0));
            sunVAO = new ArrayObject();

            mercury = new CreatPlanet(0.487f, 30, 30, 20.0f, 0.003025f, 47.36f, new Vector4(-20, 0, 0, 1), new Vector3(0, 0, 0));
            mercuryVAO = new ArrayObject();
            mercuryTrajectoryVAO = new ArrayObject();

            venus = new CreatPlanet(1.21f, 30, 30, 23.0f, 0.00181f, 35.02f, new Vector4(-23, 0, 0, 1), new Vector3(0, 0, 0));
            venusVAO = new ArrayObject();
            venusTrajectoryVAO = new ArrayObject();

            //---------------------земля-луна-----------------------------------------------------------

            earth = new CreatPlanet(1.30f, 30, 30, 30.0f, 0.465f, 29.783f, new Vector4(-30, 0, 0, 1), new Vector3(0, 0, 0));
            earthVAO = new ArrayObject();
            earthTrajectoryVAO = new ArrayObject();

            moon = new CreatPlanet(0.273f, 30, 30, 4.0f, 1.02f, 1.02f, new Vector4(-25.727f, 0, 0, 1), new Vector3(-30, 0, 0));
            moonVAO = new ArrayObject();
            moonTrajectoryVAO = new ArrayObject();

            //-----------------------------------------------------------------------------------------

            mars = new CreatPlanet(0.667f, 30, 30, 35.0f, 0.241172f, 24.13f, new Vector4(-35, 0, 0, 1), new Vector3(0, 0, 0));
            marsVAO = new ArrayObject();
            marsTrajectoryVAO = new ArrayObject();

            jupiter = new CreatPlanet(14.30f, 30, 30, 52.0f, 12.583f, 13.07f, new Vector4(-52, 0, 0, 1), new Vector3(0, 0, 0));
            jupiterVAO = new ArrayObject();
            jupiterTrajectoryVAO = new ArrayObject();

            saturn = new CreatPlanet(12.00f, 30, 30, 95.0f, 9.87f, 9.69f, new Vector4(-95, 0, 0, 1), new Vector3(0, 0, 0));
            saturnVAO = new ArrayObject();
            saturnTrajectoryVAO = new ArrayObject();

            uranus = new CreatPlanet(5.10f, 30, 30, 146.0f, 2.59f, 6.81f, new Vector4(-196, 0, 0, 1), new Vector3(0, 0, 0));
            uranusVAO = new ArrayObject();
            uranusTrajectoryVAO = new ArrayObject();

            neptune = new CreatPlanet(4.90f, 30, 30, 200.0f, 2.68f, 5.4349f, new Vector4(-300.0f, 0, 0, 1), new Vector3(0, 0, 0));
            neptuneVAO = new ArrayObject();
            neptuneTrajectoryVAO = new ArrayObject();

            skyboxVAO = new ArrayObject();

            #endregion

            rotationSpeed = 1.0f;
            speedMovement = 0.01f;

            VSync = VSyncMode.On; // вертикальная синхронизация
            CursorVisible = false; // показать/скрыть курсор
            CursorGrabbed = true;
        }
        #endregion

        #region методы класса

        private void CreatTexture(string filePath, TextureUnit texture, int tex)
        {
            #region LoadImage
            Image<Rgb24> image = Image.Load<Rgb24>(filePath);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            List<byte> pixels = new List<byte>(4 * image.Width * image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(image[x, y].R);
                    pixels.Add(image[x, y].G);
                    pixels.Add(image[x, y].B);
                }
            }
            #endregion

            GL.ActiveTexture(texture);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgb,
                image.Width, image.Height,
                0, PixelFormat.Rgb,
                PixelType.UnsignedByte,
                pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        private void Reshape(int w, int h, float n, float f, float angle)
        {
            float t = (float)(n * Math.Tan(angle * Math.PI / 360));
            float r = t * w / h;

            Matrix4 projMatr = new Matrix4()
            {
                M11 = n/r, M12 = 0, M13 = 0, M14 = 0, 
                M21 = 0, M22 = n/t, M23 = 0, M24 = 0,
                M31 = 0, M32 = 0, M33 = (f+n)/(n-f), M34 = -1,
                M41 = 0, M42 = 0, M43 = 2.0f*n*f/(n-f), M44 = 0
            };

            shaderProgramBS.ActiveProgram();
            shaderProgramBS.SetUniformMatrix4("projMatr", projMatr);
            shaderProgramBS.DeactiveProgram();

            shaderProgramTS.ActiveProgram();
            shaderProgramTS.SetUniformMatrix4("projMatr", projMatr);
            shaderProgramTS.DeactiveProgram();

            shaderProgramSBS.ActiveProgram();
            shaderProgramSBS.SetUniformMatrix4("projMatr", projMatr);
            shaderProgramSBS.DeactiveProgram();

            GL.Viewport(0, 0, w, h);
        }
        private void CreateVAOSphere( float[] verexes, int[] indexes, ArrayObject VAO)
        {
            var vboVert = new BufferObject(BufferType.ArrayBuffer);
            vboVert.SetData(verexes, BufferHint.DynamicDraw);

            var vboID = new BufferObject(BufferType.ElementBuffer);
            vboID.SetData(indexes, BufferHint.StaticDraw);

            int TextureCoord = shaderProgramBS.GetAttribProgram("aTexCoord");
            int VertexArray = shaderProgramBS.GetAttribProgram("aPosition");

            VAO.Activate();

            VAO.AttachBuffer(vboVert);
            VAO.AttachBuffer(vboID);

            VAO.AttribPointer(VertexArray, 3, AttribType.Float, false, 5 * sizeof(float), 0);
            VAO.AttribPointer(TextureCoord, 2, AttribType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            VAO.Deactivate();
            VAO.DisableAttribAll();
        }
        private void CreateVAOTrajectory(float[] verexes, int[] indexes, ArrayObject VAO)
        {
            var vboVert = new BufferObject(BufferType.ArrayBuffer);
            vboVert.SetData(verexes, BufferHint.StaticDraw);

            var vboID = new BufferObject(BufferType.ElementBuffer);
            vboID.SetData(indexes, BufferHint.StaticDraw);

            int VertexArray = shaderProgramTS.GetAttribProgram("aPosition");

            VAO.Activate();

            VAO.AttachBuffer(vboVert);
            VAO.AttachBuffer(vboID);

            VAO.AttribPointer(VertexArray, 3, AttribType.Float, false, 3 * sizeof(float), 0);

            VAO.Deactivate();
            VAO.DisableAttribAll();
        }
        private void CreateVAOSkybox(float[] verexes, ArrayObject VAO)
        {
            var vboVert = new BufferObject(BufferType.ArrayBuffer);
            vboVert.SetData(verexes, BufferHint.StaticDraw);

            int VertexArray = shaderProgramSBS.GetAttribProgram("aPosition");
            int TextureArray = shaderProgramSBS.GetAttribProgram("aTexCoord");

            VAO.Activate();

            VAO.AttachBuffer(vboVert);

            VAO.AttribPointer(VertexArray, 3, AttribType.Float, false, 5 * sizeof(float), 0);
            VAO.AttribPointer(TextureArray, 2, AttribType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            VAO.Deactivate();
            VAO.DisableAttribAll();
        }

        private void DrawSphere(
            TextureUnit textureNum, int texture, 
            int indexCount, ArrayObject VAO,
            Matrix4 rotation, Vector4 transform)
        {
            GL.ActiveTexture(textureNum);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            shaderProgramBS.ActiveProgram();

            shaderProgramBS.SetUniform1("texturePlanet", texture);
            shaderProgramBS.SetUniformMatrix4("rotation", rotation);
            shaderProgramBS.SetUniform4("moving", transform);
            shaderProgramBS.SetUniformMatrix4("view", Camera.View);

            VAO.Activate();
            VAO.DrawElements(PrimitiveType.Triangles, 0, indexCount, ElementType.UnsignedInt);
            VAO.Deactivate();

            shaderProgramBS.DeactiveProgram();
        }

        private void DrawTrajectory(
            ArrayObject VAO, int indexCount, Vector4 transform)
        { 
            shaderProgramTS.ActiveProgram();

            shaderProgramTS.SetUniform4("transform", transform);
            shaderProgramTS.SetUniformMatrix4("view", Camera.View);

            VAO.Activate();
            VAO.DrawElements(PrimitiveType.Lines, 0, indexCount, ElementType.UnsignedInt);
            VAO.Deactivate();

            shaderProgramTS.DeactiveProgram();
        }

        private void DrawSkybox(
            TextureUnit textureNum, int texture,
            int indexCount, ArrayObject VAO)
        {
            GL.ActiveTexture(textureNum);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            shaderProgramSBS.ActiveProgram();

            shaderProgramSBS.SetUniform1("textureCub", texture);

            VAO.Activate();
            VAO.Draw(PrimitiveType.Triangles, 0, indexCount);
            VAO.Deactivate();

            shaderProgramSBS.DeactiveProgram();
        }

        #endregion

        #region переопределение

        // Инициализировать ресурсы
        protected override void OnLoad()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.ClearColor(Color4.Black);

            // инициализация камеры
            Camera.InitCamera(
                1.0f, 0.00025f,
                new Vector3(0.0f, -100.0f, 100.0f),
                new Vector3((float)Math.PI, 0.5f, 0.0f));

            lastMousePos = new Vector2(MousePosition.X, MousePosition.Y);

            #region загрузка текстур

            GL.GenTextures(texturePlanet.Length, texturePlanet);

            CreatTexture(
               @"..\..\..\Data\Textures\2k_sun.jpg",
               TextureUnit.Texture1, texturePlanet[0]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_mercury.jpg",
               TextureUnit.Texture2, texturePlanet[1]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_venus_surface.jpg",
               TextureUnit.Texture3, texturePlanet[2]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_earth_daymap.jpg",
               TextureUnit.Texture4, texturePlanet[3]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_mars.jpg",
               TextureUnit.Texture5, texturePlanet[4]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_jupiter.jpg",
               TextureUnit.Texture6, texturePlanet[5]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_saturn.jpg",
               TextureUnit.Texture7, texturePlanet[6]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_uranus.jpg",
               TextureUnit.Texture8, texturePlanet[7]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_neptune.jpg",
               TextureUnit.Texture9, texturePlanet[8]);
            CreatTexture(
               @"..\..\..\Data\Textures\2k_moon.jpg",
               TextureUnit.Texture10, texturePlanet[9]);

            CreatTexture(
                @"..\..\..\Data\Textures\2k_stars.jpg", 
                TextureUnit.Texture11, texturePlanet[10]);

            #endregion

            #region создание планеты

            sun.CreatNewPlanet();
           
            CreateVAOSphere(sun.vertPlanet.ToArray(), sun.indexVertPlanet.ToArray(), sunVAO);

            mercury.CreatNewPlanet();
            CreateVAOSphere(mercury.vertPlanet.ToArray(), mercury.indexVertPlanet.ToArray(), mercuryVAO);
            CreateVAOTrajectory(mercury.vertTrajectory.ToArray(), mercury.indexVertTrajectory.ToArray(), mercuryTrajectoryVAO);

            venus.CreatNewPlanet();
            CreateVAOSphere(venus.vertPlanet.ToArray(), venus.indexVertPlanet.ToArray(), venusVAO);
            CreateVAOTrajectory(venus.vertTrajectory.ToArray(), venus.indexVertTrajectory.ToArray(), venusTrajectoryVAO);


            earth.CreatNewPlanet();
            CreateVAOSphere(earth.vertPlanet.ToArray(), earth.indexVertPlanet.ToArray(), earthVAO);
            CreateVAOTrajectory(earth.vertTrajectory.ToArray(), earth.indexVertTrajectory.ToArray(), earthTrajectoryVAO);

            mars.CreatNewPlanet();
            CreateVAOSphere(mars.vertPlanet.ToArray(), mars.indexVertPlanet.ToArray(), marsVAO);
            CreateVAOTrajectory(mars.vertTrajectory.ToArray(), mars.indexVertTrajectory.ToArray(), marsTrajectoryVAO);

            jupiter.CreatNewPlanet();
            CreateVAOSphere(jupiter.vertPlanet.ToArray(), jupiter.indexVertPlanet.ToArray(), jupiterVAO);
            CreateVAOTrajectory(jupiter.vertTrajectory.ToArray(), jupiter.indexVertTrajectory.ToArray(), jupiterTrajectoryVAO);

            saturn.CreatNewPlanet();
            CreateVAOSphere(saturn.vertPlanet.ToArray(), saturn.indexVertPlanet.ToArray(), saturnVAO);
            CreateVAOTrajectory(saturn.vertTrajectory.ToArray(), saturn.indexVertTrajectory.ToArray(), saturnTrajectoryVAO);

            uranus.CreatNewPlanet();
            CreateVAOSphere(uranus.vertPlanet.ToArray(), uranus.indexVertPlanet.ToArray(), uranusVAO);
            CreateVAOTrajectory(uranus.vertTrajectory.ToArray(), uranus.indexVertTrajectory.ToArray(), uranusTrajectoryVAO);

            neptune.CreatNewPlanet();
            CreateVAOSphere(neptune.vertPlanet.ToArray(), neptune.indexVertPlanet.ToArray(), neptuneVAO);
            CreateVAOTrajectory(neptune.vertTrajectory.ToArray(), neptune.indexVertTrajectory.ToArray(), neptuneTrajectoryVAO);

            moon.CreatNewPlanet();
            CreateVAOSphere(moon.vertPlanet.ToArray(), moon.indexVertPlanet.ToArray(), moonVAO);
            CreateVAOTrajectory(moon.vertTrajectory.ToArray(), moon.indexVertTrajectory.ToArray(), moonTrajectoryVAO);

            CreateVAOSkybox(skyboxVertices.ToArray(), skyboxVAO);
            #endregion

            base.OnLoad();
        }

        // Проверка изменения размера окна
        protected override void OnResize(ResizeEventArgs e)
        {
            Reshape(e.Width, e.Height, 1.0f, 100000.0f, 90.0f);
            base.OnResize(e);
        }

        // Учет обновлений в кадре
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            #region обработка ввода клавиатуры (движение + вращение сфер)

            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                if (speedMovement > 0)
                {
                    speedMovement -= 0.001f;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                speedMovement += 0.001f;
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                rotationSpeed += 0.01f;
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                if(rotationSpeed > 0)
                {
                    rotationSpeed -= 0.01f;
                }
            }
            if (KeyboardState.IsKeyPressed(Keys.E))
            {
                if (drawTrajectoryFLAG)
                {
                    drawTrajectoryFLAG = false;
                }
                else
                {
                    drawTrajectoryFLAG = true;
                }
            }
            if (KeyboardState.IsKeyPressed(Keys.R))
            {
                if (drawSystemEarthMoonFLAG)
                {
                    drawSystemEarthMoonFLAG = false;
                }
                else
                {
                    drawSystemEarthMoonFLAG = true;
                }
            }
            if (KeyboardState.IsKeyPressed(Keys.P))
            {
                if (drawPolygonFLAG)
                {
                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
                    GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
                    drawPolygonFLAG = false;
                }
                else
                {
                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                    GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);
                    drawPolygonFLAG = true;
                }
            }

            #endregion

            #region камера
            var lastpos = Camera.Position;
            var lastrot = Camera.Orientation;

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                Camera.Move(-0.1f, 0f, 0f);
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                Camera.Move(+0.1f, 0f, 0f);
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                Camera.Move(0f, +0.1f, 0f);
            }
            if (KeyboardState.IsKeyDown(Keys.Z))
            {
                Camera.Move(0f, -0.1f, 0f);
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                Camera.Move(0f, 0f, +0.1f);
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                Camera.Move(0f, 0f, -0.1f);
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Environment.Exit(1);
            }
            if (IsFocused)
            {
                Vector2 delta = lastMousePos - new Vector2(MousePosition.X, MousePosition.Y);
                lastMousePos += delta;
                Camera.AddRotation(delta.X, delta.Y);
                if (lastrot != Camera.Orientation || lastpos != Camera.Position)
                {
                    Console.Clear();
                    var p = Math.Sqrt(Math.Pow(Camera.Position.X, 2)+Math.Pow(Camera.Position.Y, 2));
                    var q = Math.Acos(Camera.Position.X / p) * 180 / Math.PI;
                    Console.WriteLine($"Полярный вектор позиции камеры: {p} условные единицы");
                    Console.WriteLine($"Полярный угол позиции камеры: {q} градусы");

                    p = Math.Sqrt(Math.Pow(Camera.Orientation.X, 2) + Math.Pow(Camera.Orientation.Y, 2));
                    q = Math.Acos(Camera.Orientation.X / p) * 180 / Math.PI;
                    Console.WriteLine($"Полярный вектор ориентации камеры: {p} условные единицы");
                    Console.WriteLine($"Полярный угол ориентации камеры: {q} градусы");
                }
                lastMousePos = new Vector2(MousePosition.X, MousePosition.Y);
            }
            #endregion

            #region анимация

            sun.CreatMovement(rotationSpeed, speedMovement);
            mercury.CreatMovement(rotationSpeed, speedMovement);
            venus.CreatMovement(rotationSpeed, speedMovement);
            earth.CreatMovement(rotationSpeed, speedMovement);
            moon.trajectoryCenter = earth.positionSphere.Xyz;
            moon.CreatMovement(rotationSpeed, speedMovement);
            mars.CreatMovement(rotationSpeed, speedMovement);
            jupiter.CreatMovement(rotationSpeed, speedMovement);
            saturn.CreatMovement(rotationSpeed, speedMovement);
            uranus.CreatMovement(rotationSpeed, speedMovement);
            neptune.CreatMovement(rotationSpeed, speedMovement);

            #endregion

            base.OnUpdateFrame(args);
        }

        // Отрисовка кадра
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Camera.GetViewMatrix();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawSkybox(TextureUnit.Texture11, texturePlanet[10], 36, skyboxVAO);

            #region отрисовка планет

            DrawSphere(TextureUnit.Texture1, texturePlanet[0], sun.indexVertPlanet.Count, sunVAO, sun.rotationSphere, sun.positionSphere);
            DrawSphere(TextureUnit.Texture4, texturePlanet[3], earth.indexVertPlanet.Count, earthVAO, earth.rotationSphere, earth.positionSphere);
            DrawSphere(TextureUnit.Texture10, texturePlanet[9], moon.indexVertPlanet.Count, moonVAO, moon.rotationSphere, moon.positionSphere);

            if (!drawSystemEarthMoonFLAG)
            {
                DrawSphere(TextureUnit.Texture2, texturePlanet[1], mercury.indexVertPlanet.Count, mercuryVAO, mercury.rotationSphere, mercury.positionSphere);
                DrawSphere(TextureUnit.Texture3, texturePlanet[2], venus.indexVertPlanet.Count, venusVAO, venus.rotationSphere, venus.positionSphere);
                DrawSphere(TextureUnit.Texture5, texturePlanet[4], mars.indexVertPlanet.Count, marsVAO, mars.rotationSphere, mars.positionSphere);
                DrawSphere(TextureUnit.Texture6, texturePlanet[5], jupiter.indexVertPlanet.Count, jupiterVAO, jupiter.rotationSphere, jupiter.positionSphere);
                DrawSphere(TextureUnit.Texture7, texturePlanet[6], saturn.indexVertPlanet.Count, saturnVAO, saturn.rotationSphere, saturn.positionSphere);
                DrawSphere(TextureUnit.Texture8, texturePlanet[7], uranus.indexVertPlanet.Count, uranusVAO, uranus.rotationSphere, uranus.positionSphere);
                DrawSphere(TextureUnit.Texture9, texturePlanet[8], neptune.indexVertPlanet.Count, neptuneVAO, neptune.rotationSphere, neptune.positionSphere);
            }

            #endregion

            #region отрисовка траекторий

            if (drawTrajectoryFLAG && !drawSystemEarthMoonFLAG)
            {
                DrawTrajectory(mercuryTrajectoryVAO, mercury.indexVertTrajectory.Count, new Vector4(mercury.trajectoryCenter, 1.0f));
                DrawTrajectory(venusTrajectoryVAO, venus.indexVertTrajectory.Count, new Vector4(venus.trajectoryCenter, 1.0f));
                DrawTrajectory(earthTrajectoryVAO, earth.indexVertTrajectory.Count, new Vector4(earth.trajectoryCenter, 1.0f));
                DrawTrajectory(moonTrajectoryVAO, moon.indexVertTrajectory.Count, new Vector4(moon.trajectoryCenter, 1.0f));
                DrawTrajectory(marsTrajectoryVAO, mars.indexVertTrajectory.Count, new Vector4(mars.trajectoryCenter, 1.0f));
                DrawTrajectory(jupiterTrajectoryVAO, jupiter.indexVertTrajectory.Count, new Vector4(jupiter.trajectoryCenter, 1.0f));
                DrawTrajectory(saturnTrajectoryVAO, saturn.indexVertTrajectory.Count, new Vector4(saturn.trajectoryCenter, 1.0f));
                DrawTrajectory(uranusTrajectoryVAO, uranus.indexVertTrajectory.Count, new Vector4(uranus.trajectoryCenter, 1.0f));
                DrawTrajectory(neptuneTrajectoryVAO, neptune.indexVertTrajectory.Count, new Vector4(neptune.trajectoryCenter, 1.0f));
            }
            if(drawTrajectoryFLAG && drawSystemEarthMoonFLAG)
            {
                DrawTrajectory(earthTrajectoryVAO, earth.indexVertTrajectory.Count, new Vector4(earth.trajectoryCenter, 1.0f));
                DrawTrajectory(moonTrajectoryVAO, moon.indexVertTrajectory.Count, new Vector4(moon.trajectoryCenter, 1.0f));
            }

            #endregion

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnFocusedChanged(FocusedChangedEventArgs e)
        {
            lastMousePos = new Vector2(MousePosition.X, MousePosition.Y);
            base.OnFocusedChanged(e);
        }
        // метод удаления загруженных ресурсов
        protected override void OnUnload()
        {
            sunVAO.Dispose();
            mercuryVAO.Dispose();
            mercuryTrajectoryVAO.Dispose();
            venusVAO.Dispose();
            venusTrajectoryVAO.Dispose();
            earthVAO.Dispose();
            earthTrajectoryVAO.Dispose();
            moonVAO.Dispose();
            moonTrajectoryVAO.Dispose();
            marsVAO.Dispose();
            marsTrajectoryVAO.Dispose();
            jupiterVAO.Dispose();
            jupiterTrajectoryVAO.Dispose();
            saturnVAO.Dispose();
            saturnTrajectoryVAO.Dispose();
            uranusVAO.Dispose();
            uranusTrajectoryVAO.Dispose();
            neptuneVAO.Dispose();
            neptuneTrajectoryVAO.Dispose();

            base.OnUnload();
        }

        #endregion
    }
}