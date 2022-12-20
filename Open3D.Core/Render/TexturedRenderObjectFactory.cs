using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Render
{
    public static class TexturedRenderObjectFactory
    {
        public static IReadOnlyCollection<TexturedVertex> CreateTexturedRectangle(float w, float h)
        {
            return new[]
            {
                // 表面
                new TexturedVertex(new Vector3(-1, -1, 0),    new Vector2(0, h)),
                new TexturedVertex(new Vector3(1, -1, 0),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, 0),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, 1, 0),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, -1, 0),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, 1, 0),      new Vector2(w, 0)),

                // 裏面
                new TexturedVertex(new Vector3(-1, -1, 0),   new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, 0),    new Vector2(w, 0)),
                new TexturedVertex(new Vector3(1, -1, 0),    new Vector2(0, h)),
                new TexturedVertex(new Vector3(1, -1, 0),    new Vector2(0, h)),
                new TexturedVertex(new Vector3(-1, 1, 0),    new Vector2(w, 0)),
                new TexturedVertex(new Vector3(1, 1, 0),     new Vector2(0, 0)),
            };
        }

        public static IReadOnlyCollection<TexturedVertex> CreateTexturedCube(float w, float h)
        {
            return new[]
            {
                new TexturedVertex(new Vector3(-1, -1, -1),   new Vector2(0, h)),
                new TexturedVertex(new Vector3(-1, -1, 1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, 1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, -1, 1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),     new Vector2(w, 0)),

                new TexturedVertex(new Vector3(1, -1, -1),    new Vector2(w, 0)),
                new TexturedVertex(new Vector3(1, 1, -1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, -1, 1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, 1, -1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, 1, 1),      new Vector2(0, h)),

                new TexturedVertex(new Vector3(-1, -1, -1),   new Vector2(w, 0)),
                new TexturedVertex(new Vector3(1, -1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, -1, 1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, -1, 1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, -1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),     new Vector2(0, h)),

                new TexturedVertex(new Vector3(-1, 1, -1),    new Vector2(w, 0)),
                new TexturedVertex(new Vector3(-1, 1, 1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, 1, -1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, 1, -1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, 1, 1),      new Vector2(0, h)),

                new TexturedVertex(new Vector3(-1, -1, -1),   new Vector2(0, h)),
                new TexturedVertex(new Vector3(-1, 1, -1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, -1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, -1, -1),    new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, 1, -1),    new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, 1, -1),     new Vector2(w, 0)),

                new TexturedVertex(new Vector3(-1, -1, 1),    new Vector2(0, h)),
                new TexturedVertex(new Vector3(1, -1, 1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(-1, 1, 1),     new Vector2(0, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),     new Vector2(w, h)),
                new TexturedVertex(new Vector3(1, 1, 1),      new Vector2(w, 0)),
            };
        }

        public static IReadOnlyCollection<TexturedVertex> CreateTexturedCube6(float textureWidth, float h)
        {
            const float tx10 = 0f;
            var tx11 = textureWidth / 6f;

            var tx20 = textureWidth / 6f;
            var tx21 = (textureWidth / 6f) * 2f;

            var tx30 = (textureWidth / 6f) * 2f;
            var tx31 = (textureWidth / 6f) * 3f;

            var tx40 = (textureWidth / 6f) * 3f;
            var tx41 = (textureWidth / 6f) * 4f;

            var tx50 = (textureWidth / 6f) * 4f;
            var tx51 = (textureWidth / 6f) * 5f;

            var tx60 = (textureWidth / 6f) * 5f;
            var tx61 = (textureWidth / 6f) * 6f;

            return new[]
            {
                new TexturedVertex(new Vector3(-1, -1, -1),  new Vector2(tx10, h)),
                new TexturedVertex(new Vector3(-1, -1, 1),   new Vector2(tx11, h)),
                new TexturedVertex(new Vector3(-1, 1, -1),   new Vector2(tx10, 0)),
                new TexturedVertex(new Vector3(-1, 1, -1),   new Vector2(tx10, 0)),
                new TexturedVertex(new Vector3(-1, -1, 1),   new Vector2(tx11, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),    new Vector2(tx11, 0)),

                new TexturedVertex(new Vector3(1, -1, -1),   new Vector2(tx21, 0)),
                new TexturedVertex(new Vector3(1, 1, -1),    new Vector2(tx20, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),    new Vector2(tx21, h)),
                new TexturedVertex(new Vector3(1, -1, 1),    new Vector2(tx21, h)),
                new TexturedVertex(new Vector3(1, 1, -1),    new Vector2(tx20, 0)),
                new TexturedVertex(new Vector3(1, 1, 1),     new Vector2(tx20, h)),

                new TexturedVertex(new Vector3(-1, -1, -1),  new Vector2(tx31, 0)),
                new TexturedVertex(new Vector3(1, -1, -1),   new Vector2(tx30, 0)),
                new TexturedVertex(new Vector3(-1, -1, 1),   new Vector2(tx31, h)),
                new TexturedVertex(new Vector3(-1, -1, 1),   new Vector2(tx31, h)),
                new TexturedVertex(new Vector3(1, -1, -1),   new Vector2(tx30, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),    new Vector2(tx30, h)),

                new TexturedVertex(new Vector3(-1, 1, -1),   new Vector2(tx41, 0)),
                new TexturedVertex(new Vector3(-1, 1, 1),    new Vector2(tx40, 0)),
                new TexturedVertex(new Vector3(1, 1, -1),    new Vector2(tx41, h)),
                new TexturedVertex(new Vector3(1, 1, -1),    new Vector2(tx41, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),    new Vector2(tx40, 0)),
                new TexturedVertex(new Vector3(1, 1, 1),     new Vector2(tx40, h)),

                new TexturedVertex(new Vector3(-1, -1, -1),  new Vector2(tx50, h)),
                new TexturedVertex(new Vector3(-1, 1, -1),   new Vector2(tx51, h)),
                new TexturedVertex(new Vector3(1, -1, -1),   new Vector2(tx50, 0)),
                new TexturedVertex(new Vector3(1, -1, -1),   new Vector2(tx50, 0)),
                new TexturedVertex(new Vector3(-1, 1, -1),   new Vector2(tx51, h)),
                new TexturedVertex(new Vector3(1, 1, -1),    new Vector2(tx51, 0)),

                new TexturedVertex(new Vector3(-1, -1, 1),   new Vector2(tx60, h)),
                new TexturedVertex(new Vector3(1, -1, 1),    new Vector2(tx61, h)),
                new TexturedVertex(new Vector3(-1, 1, 1),    new Vector2(tx60, 0)),
                new TexturedVertex(new Vector3(-1, 1, 1),    new Vector2(tx60, 0)),
                new TexturedVertex(new Vector3(1, -1, 1),    new Vector2(tx61, h)),
                new TexturedVertex(new Vector3(1, 1, 1),     new Vector2(tx61, 0)),
            };
        }
    }
}
