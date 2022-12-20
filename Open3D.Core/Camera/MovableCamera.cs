using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Model;

namespace Open3D.Camera
{
    public class MovableCamera : ModelBase, IMovableCamera
    {
        public Vector3 Distance { get; private set; }
        public Vector3 Target => Position + Distance.Length * Forward;
        public Matrix4 View => Matrix4.LookAt(Position, Target, Up);

        public MovableCamera() : this(Vector3.Zero, -Vector3.UnitZ) { }

        public MovableCamera(Vector3 position, Vector3 target) : base(position, Vector3.Zero)
        {
            SetCamera(position, target);
        }

        public override void LookAt(Vector3 target)
        {
            LookAt(target, Vector3.UnitY);
        }

        public override void LookAt(Vector3 target, Vector3 desiredUp)
        {
            LookAt(target, desiredUp, out bool isSucceeded);

            if (isSucceeded)
            {
                Distance = target - Position;
            }
        }

        public void RotateAround(Vector3 angles)
        {
            RotateAround(angles, Space.Self);
        }

        public void RotateAround(Vector3 angles, Space space)
        {
            RotateAround(Target, angles, space);
        }

        public void RotateAround(Vector3 point, Vector3 angles, Space space)
        {
            if (space == Space.World)
            {
                RotateAround(point, Vector3.UnitX, angles.X);
                RotateAround(point, Vector3.UnitY, angles.Y);
                RotateAround(point, Vector3.UnitZ, angles.Z);
            }
            else
            {
                RotateAround(point, Right, angles.X);
                RotateAround(point, Up, angles.Y);
                RotateAround(point, Forward, angles.Z);
            }
        }

        private void SetCamera(Vector3 position, Vector3 target)
        {
            if (target == position)
            {
                throw new ArgumentException(nameof(position) + "と" + nameof(target) + "が同じ値です。");
            }

            Position = position;
            Distance = target - position;
            LookAt(target);
        }
    }
}
