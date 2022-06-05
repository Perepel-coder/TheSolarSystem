using OpenTK.Windowing.Desktop;
using TheSolarSystem;

NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
{
    Title = "The Solar System",
    Size = new OpenTK.Mathematics.Vector2i(1000, 900),
    Location = new OpenTK.Mathematics.Vector2i(850, 70),
    WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable,
    WindowState = OpenTK.Windowing.Common.WindowState.Normal,
    StartVisible = true,
    StartFocused = true,

    Flags = OpenTK.Windowing.Common.ContextFlags.Default,
    Profile = OpenTK.Windowing.Common.ContextProfile.Compatability,
    APIVersion = new Version(3, 3),
    API = OpenTK.Windowing.Common.ContextAPI.OpenGL
};

using (Game game = new Game(GameWindowSettings.Default, nativeWindowSettings))
{
    Console.SetWindowSize(1, 1);
    Console.SetBufferSize(80, 80);
    Console.WindowWidth = 60;
    Console.WindowHeight = 20;
    Console.SetWindowPosition(Console.WindowLeft + 1, Console.WindowTop);
    game.Run();
}