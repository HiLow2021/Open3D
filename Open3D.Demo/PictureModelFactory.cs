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
    public class PictureModelFactory : ModelFactory, IDisposable
    {
        private readonly IList<IDisposable> disposables = new List<IDisposable>();
        private readonly IRenderer normalPyramidRenderer;
        private readonly IRenderer selectedPyramidRenderer;
        private readonly IRenderer frustumPyramidRenderer;

        public PictureModelFactory(IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(coloredSolidShader, texturedSolidShader)
        {
            var transparentWhite = new Color4(Color4.White.R, Color4.White.G, Color4.White.B, 0.5f);

            normalPyramidRenderer = new ColoredRenderObject(CreateSolidFrustum(Color4.GreenYellow), coloredSolidShader) { Mode = PrimitiveType.Lines };
            selectedPyramidRenderer = new ColoredRenderObject(CreateSolidFrustum(Color4.OrangeRed), coloredSolidShader) { Mode = PrimitiveType.Lines };
            frustumPyramidRenderer = new ColoredRenderObject(CreateSolidFrustum(transparentWhite), coloredSolidShader) { Mode = PrimitiveType.Lines };
            disposables.Add(normalPyramidRenderer);
            disposables.Add(selectedPyramidRenderer);
            disposables.Add(frustumPyramidRenderer);
        }

        public RenderableModel CreateFrustum()
        {
            return new RenderableModel(frustumPyramidRenderer, Vector3.Zero, Vector3.Zero);
        }

        public IList<ModelBase> CreatePicture(string picturePath)
        {
            var picture = new TexturedRenderObject(TexturedRenderObjectFactory.CreateTexturedRectangle(1, 1), texturedSolidShader, picturePath);

            disposables.Add(picture);

            return new[]
            {
                new SelectableModel(normalPyramidRenderer, selectedPyramidRenderer, Vector3.Zero, Vector3.Zero),
                new RenderableModel(picture, Vector3.Zero, Vector3.Zero, new Vector3(0.5f)),
            };
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var item in disposables)
            {
                item.Dispose();
            }
        }

        private IReadOnlyCollection<ColoredVertex> CreateSolidFrustum(Color4 color)
        {
            return new[]
            {
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(0, 1, 0), color),

                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(0, 1, 0), color),

                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(0, 1, 0), color),

                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(0, 1, 0), color),

                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, -1, -1), color),

                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
            };
        }
    }
}
