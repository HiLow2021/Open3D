using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Open3D.Render
{
    public static class ColoredRenderObjectFactory
    {
        public static IReadOnlyCollection<ColoredVertex> CreateOriginMarker()
        {
            return new[]
            {
                new ColoredVertex(new Vector3(0, 0, 0), Color4.Red),
                new ColoredVertex(new Vector3(1, 0, 0), Color4.Red),
                new ColoredVertex(new Vector3(0, 0, 0), Color4.Green),
                new ColoredVertex(new Vector3(0, 1, 0), Color4.Green),
                new ColoredVertex(new Vector3(0, 0, 0), Color4.Blue),
                new ColoredVertex(new Vector3(0, 0, 1), Color4.Blue)
            };
        }

        public static IReadOnlyCollection<ColoredVertex> CreateSolidLine(Color4 color)
        {
            return new[]
            {
                new ColoredVertex(new Vector3(0, 0, 0), color),
                new ColoredVertex(new Vector3(0, 0, -1), color),
            };
        }

        public static IReadOnlyCollection<ColoredVertex> CreateSolidPyramid(Color4 color)
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

                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
            };
        }

        public static IReadOnlyCollection<ColoredVertex> CreateSolidCube(Color4 color)
        {
            return new[]
            {
                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, 1, -1), color),
                new ColoredVertex(new Vector3(-1, 1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, 1, 1), color),

                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(1, 1, -1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(1, 1, -1), color),
                new ColoredVertex(new Vector3(1, 1, 1), color),

                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),

                new ColoredVertex(new Vector3(-1, 1, -1), color),
                new ColoredVertex(new Vector3(-1, 1, 1), color),
                new ColoredVertex(new Vector3(1, 1, -1), color),
                new ColoredVertex(new Vector3(1, 1, -1), color),
                new ColoredVertex(new Vector3(-1, 1, 1), color),
                new ColoredVertex(new Vector3(1, 1, 1), color),

                new ColoredVertex(new Vector3(-1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, 1, -1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(1, -1, -1), color),
                new ColoredVertex(new Vector3(-1, 1, -1), color),
                new ColoredVertex(new Vector3(1, 1, -1), color),

                new ColoredVertex(new Vector3(-1, -1, 1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(-1, 1, 1), color),
                new ColoredVertex(new Vector3(-1, 1, 1), color),
                new ColoredVertex(new Vector3(1, -1, 1), color),
                new ColoredVertex(new Vector3(1, 1, 1), color),
            };
        }

        public static IReadOnlyCollection<ColoredVertex> CreateSolidSphere(int slice, int stack, Color4 color)
        {
            var radius = 1;
            var vertices = new List<ColoredVertex>();

            for (int i = 0; i < stack; i++)
            {
                // 輪切り上部
                var upper = Math.PI / stack * i;
                var upperHeight = Math.Cos(upper);
                var upperWidth = Math.Sin(upper);

                // 輪切り下部
                var lower = Math.PI / stack * (i + 1);
                var lowerHeight = Math.Cos(lower);
                var lowerWidth = Math.Sin(lower);

                for (int j = 0; j <= slice; j++)
                {
                    // 輪切りの面を単位円としたときの座標
                    var rotor = 2 * Math.PI / slice * j;
                    var x = Math.Cos(rotor);
                    var y = Math.Sin(rotor);

                    vertices.Add(new ColoredVertex(new Vector3((float)(radius * x * lowerWidth), (float)(radius * lowerHeight), (float)(radius * y * lowerWidth)), color));
                    vertices.Add(new ColoredVertex(new Vector3((float)(radius * x * upperWidth), (float)(radius * upperHeight), (float)(radius * y * upperWidth)), color));
                }
            }

            return vertices;
        }

        public static IReadOnlyCollection<ColoredVertex> CreateSolidTerrain(int size, Color4 color)
        {
            var vertices = new List<ColoredVertex>();

            for (int i = -size; i <= size; i++)
            {
                vertices.Add(new ColoredVertex(new Vector3(i, 0, -size), color));
                vertices.Add(new ColoredVertex(new Vector3(i, 0, size), color));
                vertices.Add(new ColoredVertex(new Vector3(-size, 0, i), color));
                vertices.Add(new ColoredVertex(new Vector3(size, 0, i), color));
            }

            return vertices;
        }
    }
}
