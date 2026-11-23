using OpenGLCamera.Core;

namespace OpenGLCamera;

public class Program
{
    public static void Main(string[] args)
    {
        using var window = new Window(800, 600, "OpenGL Camera");
        window.Run();
    }
}