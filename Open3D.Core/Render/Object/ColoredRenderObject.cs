using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Open3D.Shader;

namespace Open3D.Render.Object
{
    public class ColoredRenderObject : RenderObjectBase
    {
        public ColoredRenderObject(IReadOnlyCollection<ColoredVertex> vertices, IShaderProgram shaderProgram)
            : base(shaderProgram, vertices.Count)
        {
            GL.NamedBufferStorage(
                vertexBuffer,
                ColoredVertex.Size * vertices.Count,
                vertices.ToArray(),
                BufferStorageFlags.MapWriteBit
            );

            GL.VertexArrayAttribBinding(vertexArray, 0, 0);
            GL.EnableVertexArrayAttrib(vertexArray, 0);
            GL.VertexArrayAttribFormat(
                vertexArray,
                0,                      // Attribute index
                4,                      // Number of elements in attribute
                VertexAttribType.Float, // Type of attribute
                false,                  // Normalize?
                0                       // Relative offset from first item
            );

            GL.VertexArrayAttribBinding(vertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(vertexArray, 1);
            GL.VertexArrayAttribFormat(
                vertexArray,
                1,                      // Attribute index
                4,                      // Number of elements in attribute
                VertexAttribType.Float, // Type of Attribute
                false,                  // Normalize?
                sizeof(float) * 4       // Relative offset from first item
            );

            // Link vao to vbo
            GL.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, IntPtr.Zero, ColoredVertex.Size);
        }
    }
}
