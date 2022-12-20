using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Open3D.Shader;

namespace Open3D.Render.Object.Old30
{
    public class ColoredRenderObject : RenderObjectBase
    {
        public ColoredRenderObject(IReadOnlyCollection<ColoredVertex> vertices, IShaderProgram shaderProgram)
            : base(shaderProgram, vertices.Count)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, ColoredVertex.Size * vertices.Count, vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
