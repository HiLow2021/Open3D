using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Camera
{
    public class StaticCamera : ICamera
    {
        public Vector3 Position { get; }
        public Vector3 Target { get; }
        public Matrix4 View { get; }

        public StaticCamera() : this(Vector3.Zero, -Vector3.UnitZ) { }

        public StaticCamera(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
            View = Matrix4.LookAt(position, target, Vector3.UnitY);
        }
    }
}
