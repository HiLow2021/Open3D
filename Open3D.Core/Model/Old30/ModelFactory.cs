using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Open3D.Render;
using Open3D.Render.Object.Old30;
using Open3D.Shader;

namespace Open3D.Model.Old30
{
    public class ModelFactory : IDisposable
    {
        protected readonly IShaderProgram coloredSolidShader;
        protected readonly IShaderProgram texturedSolidShader;
        protected readonly IRenderer originMarkerRenderer;

        public ModelFactory(IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader)
        {
            this.coloredSolidShader = coloredSolidShader;
            this.texturedSolidShader = texturedSolidShader;

            originMarkerRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateOriginMarker(), coloredSolidShader) { Mode = PrimitiveType.Lines };
        }

        public ModelBase CreateOriginMarker(float length)
        {
            return new RenderableModel(originMarkerRenderer, Vector3.Zero, Vector3.Zero, new Vector3(length));
        }

        public virtual void Dispose()
        {
            originMarkerRenderer?.Dispose();
        }
    }
}
