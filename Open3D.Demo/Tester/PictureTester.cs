using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using Open3D;
using Open3D.Controls;
using Open3D.Model;
using Open3D.Shader;

namespace Open3D.Demo.Tester
{
    public class PictureTester : TesterBase
    {
        private readonly PictureModelFactory pictureModelFactory;
        private readonly IList<ModelBase> pictures = new List<ModelBase>();
        private readonly ModelBase robot;
        private readonly ModelBase frustum;

        public PictureTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
        {
            open3DViewer.KeyDown += (sender, e) =>
            {
                KeyInput(e.KeyData);
                open3DViewer.Refresh();
            };
            open3DViewer.RayCastHit += (sender, e) =>
            {
                if (e.Value is SelectableModel s)
                {
                    s.ChangeRenderer();
                }
            };

            pictureModelFactory = new PictureModelFactory(coloredSolidShader, texturedSolidShader);
            robot = testModelFactory.CreateCube();

            var originMarker = testModelFactory.CreateOriginMarker();

            frustum = pictureModelFactory.CreateFrustum();

            originMarker.Parent = robot;
            frustum.Position = robot.Forward;
            frustum.EulerAngles = new Vector3(90, 0, 0);
            frustum.Parent = robot;

            open3DViewer.Models.Add(testModelFactory.CreateTerrain());
            open3DViewer.Models.Add(robot);
            open3DViewer.Models.Add(originMarker);
            open3DViewer.Models.Add(frustum);
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.W)
            {
                robot.Translate(-Vector3.UnitZ);
            }
            if (keys == Keys.S)
            {
                robot.Translate(Vector3.UnitZ);
            }
            if (keys == Keys.A)
            {
                robot.Translate(-Vector3.UnitX);
            }
            if (keys == Keys.D)
            {
                robot.Translate(Vector3.UnitX);
            }
            if (keys == Keys.Q)
            {
                robot.Translate(-Vector3.UnitY);
            }
            if (keys == Keys.E)
            {
                robot.Translate(Vector3.UnitY);
            }
            if (keys == Keys.R)
            {
                robot.Rotate(new Vector3(5, 0, 0), Space.World);
            }
            if (keys == Keys.T)
            {
                robot.Rotate(new Vector3(0, 5, 0), Space.World);
            }
            if (keys == Keys.Y)
            {
                robot.Rotate(new Vector3(0, 0, 5), Space.World);
            }
            if (keys == Keys.F)
            {
                robot.Rotate(new Vector3(5, 0, 0), Space.Self);
            }
            if (keys == Keys.G)
            {
                robot.Rotate(new Vector3(0, 5, 0), Space.Self);
            }
            if (keys == Keys.H)
            {
                robot.Rotate(new Vector3(0, 0, 5), Space.Self);
            }
            if (keys == Keys.L)
            {
                robot.LookAt(Vector3.Zero);
            }
            if (keys == Keys.Z)
            {
                frustum.LocalScale = new Vector3(frustum.LocalScale.X, frustum.LocalScale.Y + 0.1f, frustum.LocalScale.Z);
                frustum.LocalPosition = robot.Forward * frustum.LocalScale.Y;
            }
            if (keys == Keys.X)
            {
                frustum.LocalScale = new Vector3(frustum.LocalScale.X, frustum.LocalScale.Y - 0.1f, frustum.LocalScale.Z);
                frustum.LocalPosition = robot.Forward * frustum.LocalScale.Y;
            }
            if (keys == Keys.Enter)
            {
                foreach (var item in pictures)
                {
                    open3DViewer.Models.Remove(item);
                }

                pictures.Clear();
            }
            if (keys == Keys.Space)
            {
                TakePicture();
            }
            if (keys == Keys.M)
            {
                RandomTakePictures();
            }
        }

        private void TakePicture()
        {
            var texturePath = @"Texture\DSC07315_thumb.bmp";

            foreach (var item in pictureModelFactory.CreatePicture(texturePath))
            {
                if (item is SelectableModel)
                {
                    item.Position = frustum.Position;
                    item.Rotation = frustum.Rotation;
                    item.LocalScale = frustum.LocalScale;
                }
                else
                {
                    item.Position = frustum.Position;
                    item.Rotation = frustum.Rotation;
                    item.LocalScale *= 2;
                }

                open3DViewer.Models.Add(item);
                pictures.Add(item);
            }
        }

        private void RandomTakePictures()
        {
            var count = 1000;
            var range = 100;

            for (int i = 0; i < count; i++)
            {
                robot.LocalPosition = new Vector3(rnd.Next(-range, range), rnd.Next(-range, range), rnd.Next(-range, range));
                robot.LocalEulerAngles = new Vector3(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));

                TakePicture();
            }
        }
    }
}
