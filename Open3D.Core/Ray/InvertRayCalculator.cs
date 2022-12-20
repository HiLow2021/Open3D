using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Camera;

namespace Open3D.Ray
{
    public class InvertRayCalculator : IRayCalculator
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public ICamera Camera { get; set; }
        public Matrix4 Projection { get; set; }

        public InvertRayCalculator(int displayWidth, int displayHeight, ICamera camera, Matrix4 projection)
        {
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            Camera = camera;
            Projection = projection;
        }

        public Ray CalculateRay(int mouseX, int mouseY)
        {
            // 参考資料 http://antongerdelan.net/opengl/raycasting.html

            var x = 2.0f * mouseX / DisplayWidth - 1.0f;
            var y = 2.0f * mouseY / DisplayHeight - 1.0f;
            var rayNDC = new Vector3(x, -y, 1.0f);                          // NormalizedDeviceCoordinates;
            var rayClip = new Vector4(rayNDC.X, rayNDC.Y, -1.0f, 1.0f);     // 4D homogeneous clip coordinates
            var rayView = rayClip * Projection.Inverted();                  // 4D view (camera) coordinates

            rayView = new Vector4(rayView.X, rayView.Y, -1.0f, 0.0f);

            var rayWorld = (rayView * Camera.View.Inverted()).Xyz;

            return new Ray(Camera.Position, rayWorld.Normalized());
        }

        public Vector3 CalculateRayFromMouse2(int mouseX, int mouseY)
        {
            // heavily influenced by: http://antongerdelan.net/opengl/raycasting.html

            float x = mouseX / (DisplayWidth * 0.5f) - 1.0f;
            float y = mouseY / (DisplayHeight * 0.5f) - 1.0f;

            var invertedVP = (Camera.View * Projection).Inverted();
            var screenPosition = new Vector4(x, -y, 1.0f, 1.0f);
            var worldPosition = screenPosition * invertedVP;
            var ray = new Vector3(worldPosition);

            ray.Normalize();

            return ray;
        }
    }
}
