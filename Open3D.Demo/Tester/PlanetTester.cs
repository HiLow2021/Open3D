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
using Open3D.Ray;
using Open3D.Render;
using Open3D.Render.Object;
using Open3D.Shader;

namespace Open3D.Demo.Tester
{
    public class PlanetTester : TesterBase
    {
        private readonly Timer timer2 = new Timer();
        private readonly ModelBase parentCube;
        private readonly ModelBase childCube;
        private readonly ModelBase grandChildCube;
        private readonly ModelBase targetSphere;
        private readonly Queue<ModelBase> lasers = new Queue<ModelBase>();
        private bool isRunning;
        private int speed = 1;

        public PlanetTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            timer.Tick += (sender, e) =>
            {
                Update();
                open3DViewer.Refresh();
            };
            timer2.Interval = 10;
            timer2.Tick += (sender, e) =>
            {
                UpdateLaser();
                open3DViewer.Refresh();
            };
            open3DViewer.KeyDown += (sender, e) =>
            {
                KeyInput(e.KeyData);
                open3DViewer.Refresh();
            };
            open3DViewer.KeyUp += (sender, e) => speed = 1;

            parentCube = testModelFactory.CreateCube();
            childCube = testModelFactory.CreateCube();
            grandChildCube = testModelFactory.CreateCube();
            targetSphere = testModelFactory.CreateSphere();

            var parentOriginMarker = testModelFactory.CreateOriginMarker();
            var childOriginMarker = testModelFactory.CreateOriginMarker();
            var grandChildOriginMarker = testModelFactory.CreateOriginMarker();

            parentCube.Position = new Vector3(5, 1, 3);
            childCube.Position = new Vector3(13, 2, 7);
            grandChildCube.Position = new Vector3(-20, -5, 5);
            targetSphere.Position = new Vector3(10, 10, -5);

            parentOriginMarker.Parent = parentCube;
            childOriginMarker.Parent = childCube;
            grandChildOriginMarker.Parent = grandChildCube;
            childCube.Parent = parentCube;
            grandChildCube.Parent = childCube;

            //open3DViewer.Models.Add(testModelFactory.CreateTestTerrain());
            open3DViewer.Models.Add(testModelFactory.CreateOriginMarker(1000));
            open3DViewer.Models.Add(testModelFactory.CreateOriginMarker(-1000));
            open3DViewer.Models.Add(parentCube);
            open3DViewer.Models.Add(parentOriginMarker);
            open3DViewer.Models.Add(childCube);
            open3DViewer.Models.Add(childOriginMarker);
            open3DViewer.Models.Add(grandChildCube);
            open3DViewer.Models.Add(grandChildOriginMarker);
            open3DViewer.Models.Add(targetSphere);
            open3DViewer.Refresh();
            timer2.Start();
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.A)
            {
                SetLaser(parentCube);
            }
            if (keys == Keys.S)
            {
                SetLaser(childCube);
            }
            if (keys == Keys.D)
            {
                SetLaser(grandChildCube);
            }
            if (keys == Keys.J)
            {
                parentCube.LookAt(targetSphere.Position);
            }
            if (keys == Keys.K)
            {
                childCube.LookAt(targetSphere.Position);
            }
            if (keys == Keys.L)
            {
                grandChildCube.LookAt(targetSphere.Position);
            }
            if (keys == Keys.Enter)
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
            if (keys == Keys.Space)
            {
                speed = 10;
            }
        }

        private void SetLaser(ModelBase model)
        {
            var laser = testModelFactory.CreateLine();

            laser.Position = model.Position;
            laser.EulerAngles = model.EulerAngles;
            laser.LocalScale = new Vector3(1, 1, 1);

            lasers.Enqueue(laser);
            open3DViewer.Models.Add(laser);

            if (lasers.Count > 100)
            {
                open3DViewer.Models.Remove(lasers.Dequeue());
            }
        }

        private void Update()
        {
            for (int i = 0; i < speed; i++)
            {
                parentCube.RotateAround(Vector3.UnitY, 1);
                childCube.RotateAround(Vector3.UnitX, 1);
                grandChildCube.RotateAround(Vector3.UnitZ, 2);
            }
        }

        private void UpdateLaser()
        {
            foreach (var item in lasers)
            {
                item.Translate(-Vector3.UnitZ);
            }
        }
    }
}
