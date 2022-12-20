using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Open3D.Model;
using Open3D.Render;
using Open3D.Render.Object;
using Open3D.Shader;

namespace Open3D.Demo
{
    public class TestModelFactory : ModelFactory, IDisposable
    {
        private readonly IRenderer terrainRenderer;
        private readonly IRenderer lineRenderer;
        private readonly IRenderer cubeRenderer;
        private readonly IRenderer sphereRenderer;

        public TestModelFactory(IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(coloredSolidShader, texturedSolidShader)
        {
            terrainRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidTerrain(100, Color4.GreenYellow), coloredSolidShader) { Mode = PrimitiveType.Lines };
            lineRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidLine(Color4.OrangeRed), coloredSolidShader) { Mode = PrimitiveType.Lines };
            cubeRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidCube(Color4.Aquamarine), coloredSolidShader) { Mode = PrimitiveType.Lines };
            sphereRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidSphere(10, 10, Color4.Aquamarine), coloredSolidShader) { Mode = PrimitiveType.LineLoop };
        }

        public RenderableModel CreateTerrain()
        {
            return new RenderableModel(terrainRenderer, Vector3.Zero, Vector3.Zero);
        }

        public RenderableModel CreateOriginMarker()
        {
            return (RenderableModel)CreateOriginMarker(0.25f);
        }

        public RenderableModel CreateLine()
        {
            return new RenderableModel(lineRenderer, Vector3.Zero, Vector3.Zero, new Vector3(0.25f));
        }

        public RenderableModel CreateCube()
        {
            return new RenderableModel(cubeRenderer, Vector3.Zero, Vector3.Zero, new Vector3(0.25f));
        }

        public RenderableModel CreateSphere()
        {
            return new RenderableModel(sphereRenderer, Vector3.Zero, Vector3.Zero, new Vector3(0.25f));
        }

        public override void Dispose()
        {
            base.Dispose();
            terrainRenderer?.Dispose();
            lineRenderer?.Dispose();
            cubeRenderer?.Dispose();
            sphereRenderer?.Dispose();
        }
    }
}
