using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenGLCamera.Shaders;
using System.Diagnostics;
using OpenGLCamera.Textures;

namespace OpenGLCamera.Core;

public class Window(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
{
    private int _vertexBufferObject; // VBO
    private int _vertexArrayObject; // VAO
    private int _elementBufferObject; // EBO
    private Shader? _shader;
    private bool _wireframe;

    private Stopwatch _timer = new();

    private float[] _vertices = {
         // positions        // texture coords
        0.5f,  0.5f, 0.0f,   1.0f, 1.0f,
        0.5f, -0.5f, 0.0f,   1.0f, 0.0f,   
        -0.5f, -0.5f, 0.0f,  0.0f, 0.0f,   
        -0.5f,  0.5f, 0.0f,  0.0f, 1.0f,    
    };

    private float[] _verticesTriangle = {
        // positions        // colors
        0.0f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // haut : rouge
        0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bas droite : vert
        -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // bas gauche : bleu
    };

    private float[] texCoords = {
        0.0f, 0.0f,  // lower-left corner  
        1.0f, 0.0f,  // lower-right corner
        0.5f, 1.0f   // top-center corner
    };
    private uint[] _indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    private float[] getVertices()
    {
        return _vertices;
    }


    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        if (KeyboardState.IsKeyPressed(Keys.W))
        {
            _wireframe = !_wireframe;
            GL.PolygonMode(TriangleFace.FrontAndBack, _wireframe ? PolygonMode.Line : PolygonMode.Fill);
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _timer.Start();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        _vertexBufferObject = GL.GenBuffer();
        _vertexArrayObject = GL.GenVertexArray();
        _elementBufferObject = GL.GenBuffer();

        // Load and compile the shader program
        _shader = new Shader("Shaders/Vertices/shader.vert", "Shaders/Colors/shader.frag");
        var texture = new Texture("Textures/Files/wall.jpg");
        // Bind the VAO
        GL.BindVertexArray(_vertexArrayObject);

        // Bind the VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, getVertices().Length * sizeof(float), getVertices(), BufferUsageHint.StaticDraw);

        // Save the position attribute location in the shader and enable it
        // Save the VBO in the VAO
        int positionLocation = 0;
        int stripe = 5 * sizeof(float); // 3 for position, 2 for texture coords
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, stripe, 0);
        GL.EnableVertexAttribArray(positionLocation);

        // Save the color attribute location in the shader and enable it
        // int colorLocation = 1;
        // GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, stripe, 3 * sizeof(float));
        // GL.EnableVertexAttribArray(colorLocation);

        int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        // Bind the EBO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // Get the maximum number of vertex attributes supported by the GPU (16 here)
        // int nrAttributes = 0;
        // GL.GetInteger(GetPName.MaxVertexAttribs, out nrAttributes);
        // Console.WriteLine("Maximum number of vertex attributes supported: " + nrAttributes);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.BindVertexArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.UseProgram(0);
        
        _shader?.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        // clear the colorbuffer
        GL.Clear(ClearBufferMask.ColorBufferBit);
        // be sure to activate the shader
        _shader?.Use();
        
        // now render the rectangle
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        // swap buffers
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

    }
    
}
