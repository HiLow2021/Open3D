using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Open3D;
using Open3D.Controls;
using Open3D.Model;
using Open3D.Shader;

namespace Open3D.Demo.Tester
{
    public class RotationTester : TesterBase
    {
        private readonly IList<RenderableModel> cubes1 = new List<RenderableModel>();
        private readonly IList<RenderableModel> cubes2 = new List<RenderableModel>();
        private RenderableModel firstInLine1;
        private RenderableModel lastInLine1;
        private RenderableModel firstInLine2;
        private RenderableModel lastInLine2;
        private bool isRunning;

        public RotationTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            timer.Tick += (sender, e) =>
            {

            };
            open3DViewer.RayCastHit += (sender, e) =>
            {
                if (e.Value is ModelBase m)
                {
                    var position = m.LocalPosition;

                    Console.WriteLine($"X:{position.X} Y:{position.Y} Z:{position.Z}");
                }
            };
            open3DViewer.KeyDown += (sender, e) =>
            {
                KeyInput(e.KeyData);
                open3DViewer.Refresh();
            };

            var originMarker = testModelFactory.CreateOriginMarker();

            originMarker.LocalScale *= 400;

            open3DViewer.Models.Add(originMarker);
            open3DViewer.Models.Add(testModelFactory.CreateTerrain());

            Initialize();
            open3DViewer.Refresh();
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.A)
            {
                ApplyOffset();
            }
            if (keys == Keys.S)
            {
                ApplyRotationToZ();
            }
            if (keys == Keys.D)
            {
                ApplyRotationToX2();
            }
            if (keys == Keys.F)
            {
                ApplyRotationAroundX();
            }
            if (keys == Keys.G)
            {
                ApplyRotationAroundY();
            }
            if (keys == Keys.H)
            {
                ApplyRotationAroundZ();
            }
            if (keys == Keys.M)
            {
                for (int i = 0; i < cubes1.Count; i++)
                {
                    var distance = cubes1[i].LocalPosition - cubes2[i].LocalPosition;

                    Console.WriteLine(distance.Length);
                }

                Console.WriteLine("おしまい");
            }
            if (keys == Keys.C)
            {
                Clear();
                Initialize();
            }
            if (keys == Keys.Enter)
            {
                Clear();
                Initialize();
                ApplyOffset();
                ApplyRotationToZ();
                ApplyRotationToX();
            }
            if (keys == Keys.Space)
            {
                if (isRunning)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }

                isRunning = !isRunning;
            }
        }

        private void ApplyOffset()
        {
            var offset = firstInLine1.LocalPosition;

            foreach (var item in open3DViewer.Models.Skip(2))
            {
                item.LocalPosition -= offset;
            }
        }

        private void ApplyRotationToZ()
        {
            var direction = firstInLine1.LocalPosition - lastInLine1.LocalPosition;
            var rotation = QuaternionHelper.FromTo(-direction, -Vector3.UnitZ);

            foreach (var item in open3DViewer.Models.Skip(2))
            {
                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void ApplyRotationToX()
        {
            foreach (var item in cubes2)
            {
                var direction = item.LocalPosition - new Vector3(0, 0, item.LocalPosition.Z);
                var rotation = QuaternionHelper.FromTo(-direction, Vector3.UnitX);

                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void ApplyRotationToX2()
        {
            var direction = firstInLine2.LocalPosition - new Vector3(0, 0, firstInLine2.LocalPosition.Z);
            var rotation = QuaternionHelper.FromTo(-direction, Vector3.UnitX);

            foreach (var item in cubes2)
            {
                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void ApplyRotationAroundX()
        {
            var direction = firstInLine1.LocalPosition - lastInLine1.LocalPosition;
            var rotation = QuaternionHelper.FromTo(-direction, -Vector3.UnitZ);

            foreach (var item in open3DViewer.Models.Skip(2))
            {
                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void ApplyRotationAroundY()
        {
            var direction = firstInLine1.LocalPosition;
            var rotation = QuaternionHelper.FromTo(-direction, -Vector3.UnitZ);

            foreach (var item in open3DViewer.Models.Skip(2))
            {
                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void ApplyRotationAroundZ()
        {
            var direction = firstInLine1.LocalPosition - lastInLine1.LocalPosition;
            var rotation = QuaternionHelper.FromTo(-direction, Vector3.UnitY);

            foreach (var item in open3DViewer.Models.Skip(2))
            {
                item.LocalPosition = rotation * item.LocalPosition;
            }
        }

        private void Clear()
        {
            foreach (var item in cubes1)
            {
                open3DViewer.Models.Remove(item);
            }
            foreach (var item in cubes2)
            {
                open3DViewer.Models.Remove(item);
            }

            cubes1.Clear();
            cubes2.Clear();
        }

        private void Initialize()
        {
            var direction = new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), 0);
            var offsetRange = 100;
            var offset = new Vector3(rnd.Next(offsetRange), rnd.Next(offsetRange), 0);

            direction.X = (rnd.Next(2) == 0) ? direction.X * -1 : direction.X;
            direction.Y = (rnd.Next(2) == 0) ? direction.Y * -1 : direction.Y;
            direction.Z = (rnd.Next(2) == 0) ? direction.Z * -1 : direction.Z;

            for (int i = 0; i < 100; i++)
            {
                var cube = testModelFactory.CreateCube();

                cube.LocalPosition = offset + direction * i;
                cubes1.Add(cube);
                open3DViewer.Models.Add(cube);
            }

            var normal = Vector3.Cross(direction, -Vector3.UnitZ).Normalized();
            var dummy = Vector3.Cross(direction, normal).Normalized() * 0.2f;

            for (int i = 0; i < 100; i++)
            {
                var cube = testModelFactory.CreateCube();

                cube.LocalPosition = offset + normal + new Vector3(direction.X + 0.01f, direction.Y + 0.02f, direction.Z - 0.01f) * i;
                cubes2.Add(cube);
                open3DViewer.Models.Add(cube);
            }

            firstInLine1 = cubes1[0];
            lastInLine1 = cubes1[cubes1.Count - 1];
            firstInLine2 = cubes2[0];
            lastInLine2 = cubes2[cubes2.Count - 1];
        }
    }
}
