using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Model;

namespace Open3D.Camera
{
    public class FirstPersonCamera : ICamera
    {
        public Vector3 Position { get; private set; }
        public Vector3 Target => TargetModel.Position;
        public ModelBase TargetModel { get; private set; }
        public Matrix4 View { get; private set; }
        public Vector3 Offset { get; private set; }

        public FirstPersonCamera(ModelBase target) : this(target, Vector3.Zero) { }
        public FirstPersonCamera(ModelBase target, Vector3 offset)
        {
            TargetModel = target;
            Offset = offset;
        }

        public void Update(double time, double delta)
        {
            //var position = new Vector3(Target.Position) + Offset;

            //Position = position;
            //LookAtMatrix = Matrix4.LookAt(
            //    position,
            //    new Vector3(Target.Position + Target.Direction) + Offset,
            //    Vector3.UnitY);
        }
    }
}
