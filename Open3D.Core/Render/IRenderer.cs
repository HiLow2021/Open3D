using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D.Render
{
    public interface IRenderer : IDisposable
    {
        int ModelParameterId { get; }
        int ViewParameterId { get; }
        int ProjectionParameterId { get; }

        void Bind();
        void Render();
    }
}
