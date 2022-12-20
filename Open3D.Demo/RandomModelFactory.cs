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
    public class RandomModelFactory : ModelFactory, IDisposable
    {
        private readonly Random rnd = new Random();
        private readonly IRenderer lineRenderer;
        private readonly IRenderer normalCubeRenderer;
        private readonly IRenderer selectedCubeRenderer;
        private readonly IRenderer normalSphereRenderer;
        private readonly IRenderer selectedSphereRenderer;

        public RandomModelFactory(IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(coloredSolidShader, texturedSolidShader)
        {
            lineRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidLine(Color4.Aqua), coloredSolidShader) { Mode = PrimitiveType.Lines };
            normalCubeRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidCube(Color4.Aquamarine), coloredSolidShader) { Mode = PrimitiveType.Lines };
            selectedCubeRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidCube(Color4.OrangeRed), coloredSolidShader) { Mode = PrimitiveType.Lines };
            normalSphereRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidSphere(10, 10, Color4.Aquamarine), coloredSolidShader) { Mode = PrimitiveType.Lines };
            selectedSphereRenderer = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidSphere(10, 10, Color4.OrangeRed), coloredSolidShader) { Mode = PrimitiveType.Lines };
        }

        public RenderableModel CreateRandomSolidLine()
        {
            return new RenderableModel(lineRenderer, GetRandomPosition(100), new Vector3(90, 0, 0), new Vector3(3));
        }

        public SelectableModel CreateRandomSolidCube()
        {
            return new SelectableModel(normalCubeRenderer, selectedCubeRenderer, GetRandomPosition(100), Vector3.Zero, new Vector3(0.25f));
        }

        public SelectableModel CreateRandomSolidSphere()
        {
            return new SelectableModel(normalSphereRenderer, selectedSphereRenderer, GetRandomPosition(100), Vector3.Zero, new Vector3(0.25f));
        }

        public override void Dispose()
        {
            base.Dispose();
            lineRenderer?.Dispose();
            normalCubeRenderer?.Dispose();
            selectedCubeRenderer?.Dispose();
            normalSphereRenderer?.Dispose();
            selectedSphereRenderer?.Dispose();
        }

        private Vector3 GetRandomPosition(int range)
        {
            return new Vector3(rnd.Next(-range, range), rnd.Next(-range, range), rnd.Next(-range, range));
        }
    }
}
