using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Open3D.Render;

namespace Open3D.Model
{
    public class RenderableModel : ModelBase, IRenderableModel
    {
        public IRenderer Renderer { get; protected set; }

        public RenderableModel(IRenderer renderer) : base()
        {
            Renderer = renderer;
        }

        public RenderableModel(IRenderer renderer, Vector3 position) : base(position)
        {
            Renderer = renderer;
        }

        public RenderableModel(IRenderer renderer, Vector3 position, Vector3 rotation) : base(position, rotation)
        {
            Renderer = renderer;
        }

        public RenderableModel(IRenderer renderer, Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {
            Renderer = renderer;
        }

        public virtual void Render()
        {
            Renderer.Bind();

            var model = Model;

            GL.UniformMatrix4(Renderer.ModelParameterId, false, ref model);

            Renderer.Render();
        }
    }
}
