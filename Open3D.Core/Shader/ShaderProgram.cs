using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Open3D.Shader
{
    public class ShaderProgram : IShaderProgram, IDisposable
    {
        public int Id { get; private set; }
        public int ModelParameterId { get; }
        public int ViewParameterId { get; }
        public int ProjectionParameterId { get; }

        public ShaderProgram(Dictionary<ShaderType, string> shaders, int modelParameterId, int viewParameterId, int projectionParameterId)
        {
            Initialize(shaders);
            ModelParameterId = modelParameterId;
            ViewParameterId = viewParameterId;
            ProjectionParameterId = projectionParameterId;
        }

        public ShaderProgram(Dictionary<ShaderType, string> shaders, string modelParameterName, string viewParameterName, string projectionParameterName)
        {
            Initialize(shaders);
            ModelParameterId = GL.GetUniformLocation(Id, modelParameterName);
            ViewParameterId = GL.GetUniformLocation(Id, viewParameterName);
            ProjectionParameterId = GL.GetUniformLocation(Id, projectionParameterName);
        }

        public void Bind() => GL.UseProgram(Id);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteProgram(Id);
            }
        }

        private void Initialize(Dictionary<ShaderType, string> shaders)
        {
            Id = GL.CreateProgram();

            var shaderComponentIds = shaders.Select(s =>
            {
                var shaderId = GL.CreateShader(s.Key);
                var source = File.ReadAllText(s.Value);

                GL.ShaderSource(shaderId, source);
                GL.CompileShader(shaderId);

                var shaderInfo = GL.GetShaderInfoLog(shaderId);

                if (!string.IsNullOrEmpty(shaderInfo))
                {
                    Console.WriteLine($"GL.CompileShader [{s.Key}] had info log: {shaderInfo}");
                }

                GL.AttachShader(Id, shaderId);

                return shaderId;
            }).ToArray(); // Need to force linq to iterate over the collection so we actually create shaders...

            GL.LinkProgram(Id);

            var programInfo = GL.GetProgramInfoLog(Id);

            if (!string.IsNullOrEmpty(programInfo))
            {
                Console.WriteLine($"GL.LinkProgram had info log {programInfo}");
            }

            foreach (var shaderId in shaderComponentIds)
            {
                GL.DetachShader(Id, shaderId);
                GL.DeleteShader(shaderId);
            }
        }

        private int CreateShader(ShaderType shaderType, string filePath)
        {
            var shaderId = GL.CreateShader(shaderType);
            var source = File.ReadAllText(filePath);

            // 生成されたシェーダにソースを割り当てる
            GL.ShaderSource(shaderId, source);

            // シェーダをコンパイルする
            GL.CompileShader(shaderId);

            return shaderId;
        }

        private int CreateProgram(int vertexShader, int fragmentShader)
        {
            var program = GL.CreateProgram();

            // プログラムオブジェクトにシェーダを割り当てる
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            // シェーダをリンク
            GL.LinkProgram(program);

            // プログラムオブジェクトを有効にする
            // GL.UseProgram(program);

            return program;
        }
    }
}
