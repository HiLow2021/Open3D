using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D.Shader
{
    public interface IShaderProgram : IDisposable
    {
        int ModelParameterId { get; }
        int ViewParameterId { get; }
        int ProjectionParameterId { get; }

        void Bind();
    }
}
