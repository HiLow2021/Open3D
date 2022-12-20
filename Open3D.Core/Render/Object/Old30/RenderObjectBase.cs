using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Open3D.Shader;

namespace Open3D.Render.Object.Old30
{
    public abstract class RenderObjectBase : IRenderer, IDisposable
    {
        protected readonly IShaderProgram shaderProgram;
        protected readonly int vertexArray;
        protected readonly int vertexBuffer;
        protected readonly int vertexCount;

        public PrimitiveType Mode { get; set; } = PrimitiveType.Triangles;

        public int ModelParameterId => shaderProgram.ModelParameterId;
        public int ViewParameterId => shaderProgram.ViewParameterId;
        public int ProjectionParameterId => shaderProgram.ProjectionParameterId;

        protected RenderObjectBase(IShaderProgram shaderProgram, int vertexCount)
        {
            this.shaderProgram = shaderProgram;
            this.vertexCount = vertexCount;

            GL.GenVertexArrays(1, out vertexArray);
            GL.BindVertexArray(vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        }

        public virtual void Bind()
        {
            shaderProgram.Bind();
            GL.BindVertexArray(vertexArray);
        }

        public virtual void Render() => GL.DrawArrays(Mode, 0, vertexCount);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
