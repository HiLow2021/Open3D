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
using Open3D.Render;
using Open3D.Render.Object;
using Open3D.Shader;

namespace Open3D.Demo.Tester
{
    public class MotionTester : TesterBase
    {
        private readonly ModelBase model;
        private readonly ModelBase target;
        private bool isRunning;
        private long count;

        public MotionTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            timer.Tick += (sender, e) =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    model.Rotate(new Vector3(0, 5, 0), Space.Self);
                    count++;
                }

                Console.WriteLine($"X:{model.LocalEulerAngles.X} Y:{model.LocalEulerAngles.Y} Z:{model.LocalEulerAngles.Z}");
                Console.WriteLine($"count:{count}");
                open3DViewer.Refresh();
            };
            open3DViewer.KeyDown += (sender, e) =>
            {
                KeyInput(e.KeyData);
                open3DViewer.Refresh();
            };

            model = testModelFactory.CreateCube();

            var originMarker = testModelFactory.CreateOriginMarker();

            originMarker.Parent = model;

            var targetRenderObject = new ColoredRenderObject(ColoredRenderObjectFactory.CreateSolidSphere(100, 100, Color4.OrangeRed), coloredSolidShader) { Mode = PrimitiveType.Lines };

            target = new RenderableModel(targetRenderObject, new Vector3(4, 0, 0), new Vector3(90, 0, 0), new Vector3(0.25f));

            open3DViewer.Models.Add(testModelFactory.CreateTerrain());
            open3DViewer.Models.Add(model);
            open3DViewer.Models.Add(originMarker);
            open3DViewer.Models.Add(target);
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.W)
            {
                model.Translate(-Vector3.UnitZ, Space.Self);
            }
            if (keys == Keys.S)
            {
                model.Translate(Vector3.UnitZ, Space.Self);
            }
            if (keys == Keys.A)
            {
                model.Translate(-Vector3.UnitX, Space.Self);
            }
            if (keys == Keys.D)
            {
                model.Translate(Vector3.UnitX, Space.Self);
            }
            if (keys == Keys.Q)
            {
                model.Translate(-Vector3.UnitY, Space.Self);
            }
            if (keys == Keys.E)
            {
                model.Translate(Vector3.UnitY, Space.Self);
            }
            if (keys == Keys.R)
            {
                model.Rotate(new Vector3(5, 0, 0), Space.World);
            }
            if (keys == Keys.T)
            {
                model.Rotate(new Vector3(0, 5, 0), Space.World);
            }
            if (keys == Keys.Y)
            {
                model.Rotate(new Vector3(0, 0, 5), Space.World);
            }
            if (keys == Keys.F)
            {
                model.Rotate(new Vector3(5, 0, 0), Space.Self);
            }
            if (keys == Keys.G)
            {
                model.Rotate(new Vector3(0, 5, 0), Space.Self);
            }
            if (keys == Keys.H)
            {
                model.Rotate(new Vector3(0, 0, 5), Space.Self);
            }

            if (keys == Keys.U)
            {
                model.RotateAround(target.LocalPosition, Vector3.UnitX, 5);
            }
            if (keys == Keys.I)
            {
                model.RotateAround(target.LocalPosition, Vector3.UnitY, 5);
            }
            if (keys == Keys.O)
            {
                model.RotateAround(target.LocalPosition, Vector3.UnitZ, 5);
            }
            if (keys == Keys.J)
            {
                target.RotateAround(Vector3.UnitX, 5);
            }
            if (keys == Keys.K)
            {
                target.RotateAround(Vector3.UnitY, 5);
            }
            if (keys == Keys.L)
            {
                target.RotateAround(Vector3.UnitZ, 5);
            }

            if (keys == Keys.N)
            {
                //model.Rotate(new Vector3(10, 10, 10), Space.Self);

                model.Rotate(new Vector3(0, 0, 10), Space.Self);
                model.Rotate(new Vector3(10, 0, 0), Space.Self);
                model.Rotate(new Vector3(0, 10, 0), Space.Self);
            }
            if (keys == Keys.M)
            {
                model.LocalEulerAngles = new Vector3(45, 45, 45);
            }
            if (keys == Keys.Z)
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
            if (keys == Keys.X)
            {
                model.LookAt(target.LocalPosition);
            }
            if (keys == Keys.C)
            {
                var direction = model.LocalPosition - target.LocalPosition;
                var rotation = QuaternionHelper.FromTo(-direction, -Vector3.UnitZ);

                target.LocalPosition = rotation * target.LocalPosition;
            }
            if (keys == Keys.Enter)
            {
                model.LocalEulerAngles = new Vector3(0, 0, 0);
            }
            if (keys == Keys.Space)
            {
                model.LocalEulerAngles += new Vector3(10, 10, 10);
            }

            Console.WriteLine($"X:{model.LocalEulerAngles.X} Y:{model.LocalEulerAngles.Y} Z:{model.LocalEulerAngles.Z}");
        }
    }
}
