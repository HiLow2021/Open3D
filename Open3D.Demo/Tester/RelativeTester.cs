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
    public class RelativeTester : TesterBase
    {
        private readonly PictureModelFactory pictureModelFactory;
        private readonly IList<ModelBase> pictures = new List<ModelBase>();
        private readonly ModelBase tankUpper;
        private readonly ModelBase tankUnder;
        private readonly ModelBase frustum;
        private readonly ModelBase target1;
        private readonly ModelBase target2;

        public RelativeTester(Open3DViewer open3DViewer, IShaderProgram coloredSolidShader, IShaderProgram texturedSolidShader) : base(open3DViewer, coloredSolidShader, texturedSolidShader)
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
            tankUpper = testModelFactory.CreateCube();
            tankUnder = testModelFactory.CreateCube();
            tankUnder.LocalScale = new Vector3(2, 1, 3);

            var originMarker = testModelFactory.CreateOriginMarker();
            var tankBarrel = testModelFactory.CreateCube();
            var tankTrackLeft = testModelFactory.CreateSphere();
            var tankTrackRight = testModelFactory.CreateSphere();

            tankUpper.LocalPosition = tankUnder.Up * tankUnder.LocalScale.Y * 1.5f;
            tankUpper.LocalScale = new Vector3(0.5f, 0.5f, 0.5f);
            tankUpper.Parent = tankUnder;
            tankBarrel.LocalPosition = tankUpper.Forward * tankUpper.LocalScale.Z * 4;
            tankBarrel.LocalScale = new Vector3(0.3f, 0.5f, 1.5f);
            tankBarrel.Parent = tankUpper;
            tankTrackLeft.LocalPosition = -tankUnder.Right * tankUnder.LocalScale.X * 0.6f;
            tankTrackLeft.LocalScale = new Vector3(0.2f, 0.8f, 1.1f);
            tankTrackLeft.Parent = tankUnder;
            tankTrackRight.LocalPosition = tankUnder.Right * tankUnder.LocalScale.X * 0.6f;
            tankTrackRight.LocalScale = new Vector3(0.2f, 0.8f, 1.1f);
            tankTrackRight.Parent = tankUnder;

            frustum = pictureModelFactory.CreateFrustum();

            originMarker.Parent = tankUnder;
            frustum.Position = tankBarrel.Forward * tankBarrel.LocalScale.Z;
            frustum.EulerAngles = new Vector3(90, 0, 0);
            frustum.Parent = tankBarrel;

            target1 = testModelFactory.CreateSphere();
            target1.Position = new Vector3(10);
            target2 = testModelFactory.CreateSphere();
            target2.Position = Vector3.Zero;

            //open3DViewer.Models.Add(testModelFactory.CreateTestTerrain());
            open3DViewer.Models.Add(tankUpper);
            open3DViewer.Models.Add(tankUnder);
            open3DViewer.Models.Add(tankBarrel);
            open3DViewer.Models.Add(tankTrackLeft);
            open3DViewer.Models.Add(tankTrackRight);
            open3DViewer.Models.Add(originMarker);
            open3DViewer.Models.Add(frustum);
            open3DViewer.Models.Add(target1);
            open3DViewer.Models.Add(target2);

            var eyeLine = testModelFactory.CreateLine();
            var originMarker2 = testModelFactory.CreateOriginMarker();

            var tempSphere = testModelFactory.CreateSphere();

            tempSphere.LocalPosition = new Vector3(0, 1.5f, -6.375f);

            eyeLine.LocalScale = new Vector3(1, 1, 1000);
            eyeLine.Parent = tankUpper;
            originMarker2.Parent = tankUpper;
            open3DViewer.Models.Add(originMarker2);
            open3DViewer.Models.Add(eyeLine);
            open3DViewer.Models.Add(tempSphere);
        }

        private void KeyInput(Keys keys)
        {
            if (keys == Keys.W)
            {
                tankUnder.Translate(-Vector3.UnitZ *1);
            }
            if (keys == Keys.S)
            {
                tankUnder.Translate(Vector3.UnitZ *1);
            }
            if (keys == Keys.A)
            {
                tankUnder.Translate(-Vector3.UnitX * 1);
            }
            if (keys == Keys.D)
            {
                tankUnder.Translate(Vector3.UnitX * 1);
            }
            if (keys == Keys.Q)
            {
                tankUnder.Translate(-Vector3.UnitY * 1);
            }
            if (keys == Keys.E)
            {
                tankUnder.Translate(Vector3.UnitY * 1);
            }
            if (keys == Keys.R)
            {
                tankUpper.Rotate(new Vector3(5, 0, 0), Space.Self);
            }
            if (keys == Keys.T)
            {
                tankUpper.Rotate(new Vector3(0, 5, 0), Space.Self);
            }
            if (keys == Keys.Y)
            {
                tankUpper.Rotate(new Vector3(0, 0, 5), Space.Self);
            }
            if (keys == Keys.F)
            {
                tankUnder.Rotate(new Vector3(5, 0, 0), Space.Self);
            }
            if (keys == Keys.G)
            {
                tankUnder.Rotate(new Vector3(0, 5, 0), Space.Self);
            }
            if (keys == Keys.H)
            {
                tankUnder.Rotate(new Vector3(0, 0, 5), Space.Self);
            }
            if (keys == Keys.L)
            {
                tankUpper.LookAt(target1.Position);
            }
            if (keys == Keys.Z)
            {
                frustum.LocalScale = new Vector3(frustum.LocalScale.X, frustum.LocalScale.Y + 0.1f, frustum.LocalScale.Z);
                frustum.LocalPosition = tankUnder.Forward * frustum.LocalScale.Y;
            }
            if (keys == Keys.X)
            {
                frustum.LocalScale = new Vector3(frustum.LocalScale.X, frustum.LocalScale.Y + 0.1f, frustum.LocalScale.Z);
                frustum.LocalPosition = tankUnder.Forward * frustum.LocalScale.Y;
            }
            if (keys == Keys.C)
            {
                open3DViewer.CameraMode = open3DViewer.CameraMode == CameraMode.Target ? CameraMode.Free : CameraMode.Target;
            }
            if (keys == Keys.V)
            {
                var rectangle = open3DViewer.ClientRectangle;
                var resolution = 4;
                var bitmap = open3DViewer.GetScreen(rectangle.Width * resolution, rectangle.Height * resolution);
                var now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var path = @"Pictures" + System.IO.Path.DirectorySeparatorChar + now + ".bmp";

                bitmap.Save(path);
            }
            if (keys == Keys.B)
            {
                tankUnder.LocalEulerAngles = new Vector3(0, tankUnder.LocalEulerAngles.Y, 0);
            }
            if (keys == Keys.Enter)
            {
                foreach (var item in pictures)
                {
                    open3DViewer.Models.Remove(item);
                }

                pictures.Clear();
            }

            open3DViewer.CameraRotationPoint = tankUnder.Position;
        }
    }
}
