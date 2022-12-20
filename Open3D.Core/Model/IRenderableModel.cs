using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D.Render
{
    public interface IRenderableModel
    {
        IRenderer Renderer { get; }

        void Render();
    }
}
