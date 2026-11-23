using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenGLCamera.Shaders;

namespace OpenGLCamera.Core;

public class Window(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private Shader? _shader;

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        _vertexBufferObject = GL.GenBuffer();
        _vertexArrayObject = GL.GenVertexArray();
        float[] vertices = {
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f
        };
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        _shader = new Shader("Shaders/Vertices/shader.vert", "Shaders/Colors/shader.frag");

        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(_shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        
        _shader?.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        _shader?.Use();
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

    }
}
