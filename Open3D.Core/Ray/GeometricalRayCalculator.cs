using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Camera;

namespace Open3D.Ray
{
    public class GeometricalRayCalculator : IRayCalculator
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public ICamera Camera { get; set; }
        public Matrix4 Projection { get; set; }

        public GeometricalRayCalculator(int displayWidth, int displayHeight, ICamera camera, Matrix4 projection)
        {
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            Camera = camera;
            Projection = projection;
        }

        public Ray CalculateRay(int mouseX, int mouseY)
        {
            var rad = 60f * ((float)Math.PI / 180f);
            var nearClippingPlaneDistance = 0.1f;
            var vLength = (float)Math.Tan(rad / 2) * nearClippingPlaneDistance;
            var hLength = vLength * (DisplayWidth / DisplayHeight);

            var maxX = 1.0f;
            var maxY = 1.0f;
            var minX = -1.0f;
            var minY = -1.0f;
            var dx = (maxX - minX) * mouseX / DisplayWidth + minX;
            var dy = maxY - (maxY - minY) * mouseY / DisplayHeight;

            dx *= hLength;
            dy *= vLength;

            var dz = nearClippingPlaneDistance;
            //var ray = dx * Camera.Right + dy * Camera.Up + dz * Camera.Forward;

            //ray.Normalize();

            return null;
        }
    }
}
