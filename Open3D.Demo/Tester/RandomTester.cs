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
    public class RandomTester : TesterBase
    {
        private readonly RandomModelFactory randomModelFactory;
        private readonly Dictionary<int, SelectableModel> history = new Dictionary<int, SelectableModel>();

        public RandomTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            timer.Tick += (sender, e) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    RandomTest();
                }

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

            open3DViewer.Models.Add(testModelFactory.CreateTerrain());

            randomModelFactory = new RandomModelFactory(coloredSolidShader, texturedSolidShader);
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.R)
            {
                foreach (var item in history.Values)
                {
                    var direction = Vector3.Zero - item.LocalPosition;
                    var rotation = QuaternionHelper.FromTo(-direction, -Vector3.UnitZ);

                    item.LocalPosition = rotation * item.LocalPosition;
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

        private void RandomTest()
        {
            var y = rnd.Next(2000);
            var model = randomModelFactory.CreateRandomSolidCube();

            if (history.ContainsKey(y))
            {
                var old = history[y];

                open3DViewer.Models.Remove(old);
                history[y] = model;
            }
            else
            {
                history.Add(y, model);
            }

            open3DViewer.Models.Add(model);
        }

        private void Move()
        {
            foreach (var item in history)
            {
                item.Value.Translate(new Vector3(0.0f, -0.1f, 0.0f), Space.World);
                item.Value.Rotate(new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
    }
}
