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
    public class RandomTester2 : TesterBase
    {
        private class MovableModel
        {
            public SelectableModel Model { get; set; }
            public Vector3 Velocity { get; set; }

            public MovableModel(SelectableModel model, Vector3 velocity)
            {
                Model = model;
                Velocity = velocity;
            }
        }

        private readonly RandomModelFactory randomModelFactory;
        private readonly HashSet<MovableModel> history = new HashSet<MovableModel>();

        public RandomTester2(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            timer.Tick += (sender, e) =>
            {
                Move();

                open3DViewer.Refresh();
            };
            open3DViewer.KeyDown += (sender, e) =>
            {
                KeyInput(e.KeyData);
                open3DViewer.Refresh();
            };
            open3DViewer.RayCastHit += (sender, e) =>
            {
                if (e.Value is SelectableModel sm)
                {
                    sm.ChangeRenderer();
                    open3DViewer.Refresh();
                }
            };

            var area = testModelFactory.CreateCube();

            area.LocalScale = new Vector3(400);

            open3DViewer.Models.Add(area);

            randomModelFactory = new RandomModelFactory(coloredSolidShader, texturedSolidShader);
            InitializeSolidSpheres();
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.R)
            {
                foreach (var item in history)
                {
                    item.Model.LocalPosition = Vector3.Zero;
                }
            }
            if (keys == Keys.T)
            {
                timer.Start();
            }
            if (keys == Keys.Y)
            {
                timer.Stop();
            }
        }

        private void InitializeSolidSpheres()
        {
            for (int i = 0; i < 5000; i++)
            {
                var model = randomModelFactory.CreateRandomSolidSphere();

                history.Add(new MovableModel(model, GetRandomVelocity(3)));
                open3DViewer.Models.Add(model);
            }
        }

        private void Move()
        {
            foreach (var item in history)
            {
                var model = item.Model;

                model.Translate(item.Velocity, Space.World);
                model.Rotate(new Vector3(0.5f, 0.5f, 0.5f));

                var range = 400;

                if (model.LocalPosition.X > range || model.LocalPosition.X < -range)
                {
                    item.Velocity *= new Vector3(-1, 1, 1);
                }
                if (model.LocalPosition.Y > range || model.LocalPosition.Y < -range)
                {
                    item.Velocity *= new Vector3(1, -1, 1);
                }
                if (model.LocalPosition.Z > range || model.LocalPosition.Z < -range)
                {
                    item.Velocity *= new Vector3(1, 1, -1);
                }
            }
        }

        private Vector3 GetRandomVelocity(int range)
        {
            var x = rnd.Next(-range, range);
            var y = rnd.Next(-range, range);
            var z = rnd.Next(-range, range);

            x = (x == 0 && y == 0 && z == 0) ? 1 : x;

            return new Vector3(x, y, z);
        }
    }
}
