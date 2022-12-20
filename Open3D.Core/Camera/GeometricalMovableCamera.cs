using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Camera
{
    public class GeometricalMovableCamera : IMovableCamera
    {
        public Vector3 Forward { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 Up { get; private set; }

        public Vector3 Distance => Target - Position;
        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }
        public Matrix4 View => Matrix4.LookAt(Position, Target, Up);

        public GeometricalMovableCamera() : this(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY) { }

        public GeometricalMovableCamera(Vector3 position, Vector3 target, Vector3 up)
        {
            SetCamera(position, target, up);
        }

        public void Translate(Vector3 translation)
        {
            Translate(translation, Space.Self);
        }

        public void Translate(Vector3 translation, Space space)
        {
            if (space == Space.Self)
            {
                var tx = translation.X * Right;
                var ty = translation.Y * Up;
                var tz = -translation.Z * Forward;

                translation = tx + ty + tz;
            }

            Position += translation;
            Target += translation;
        }

        public void Rotate(Vector3 angles)
        {
            Rotate(angles, Space.Self);
        }

        public void Rotate(Vector3 angles, Space space)
        {
            if (space == Space.World)
            {
                RotateAroundAxis(Vector3.UnitX, angles.X);
                RotateAroundAxis(Vector3.UnitY, angles.Y);
                RotateAroundAxis(Vector3.UnitZ, angles.Z);
            }
            else
            {
                RotateAroundAxis(Right, angles.X);
                RotateAroundAxis(Up, angles.Y);
                RotateAroundAxis(Forward, angles.Z);
            }

            Target = Position + Distance.Length * Forward;
        }

        public void RotateAround(Vector3 angles)
        {
            RotateAround(angles, Space.Self);
        }

        public void RotateAround(Vector3 angles, Space space)
        {
            Translate(Distance, Space.World);
            Rotate(-angles, space);
            Translate(-Distance, Space.World);
        }

        private void NormalizeUnits()
        {
            Right.Normalize();
            Up.Normalize();
            Forward.Normalize();
        }

        private void RotateAroundAxis(Vector3 axis, float degreeAngle)
        {
            if (axis != Right) Right = RotateAroundAxis(Right, axis, degreeAngle);
            if (axis != Up) Up = RotateAroundAxis(Up, axis, degreeAngle);
            if (axis != Forward) Forward = RotateAroundAxis(Forward, axis, degreeAngle);

            NormalizeUnits();
        }

        private Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float degreeAngle)
        {
            // axisはNormalizedしてください。

            // ret = v cos radianAngle + (axis x v) sin radianAngle + axis(axis . v)(1 - cos radianAngle)
            // (See Mathematics for 3D Game Programming and Computer Graphics, P.62, for details of why this is (it's not very hard)).
            var radianAngle = MathHelper.DegreesToRadians(degreeAngle);
            var cosAngle = (float)Math.Cos(radianAngle);
            var sinAngle = (float)Math.Sin(radianAngle);
            var aCROSSv = Vector3.Cross(axis, v);
            var ret = v;

            ret = cosAngle * ret;
            ret = sinAngle * aCROSSv + ret;
            ret = axis * (Vector3.Dot(axis, v) * (1 - cosAngle)) + ret;

            return ret;
        }

        private void SetCamera(Vector3 position, Vector3 target, Vector3 up)
        {
            if (target == position)
            {
                throw new ArgumentException(nameof(position) + "と" + nameof(target) + "が同じ値です。");
            }

            Position = position;
            Target = target;
            Forward = (target - position).Normalized();
            Up = up.Normalized();
            Right = -Vector3.Cross(up, Forward).Normalized();
        }
    }
}
