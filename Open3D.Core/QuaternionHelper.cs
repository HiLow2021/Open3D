using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D
{
    public static class QuaternionHelper
    {
        public static Vector3 ToEulerAngles(Quaternion rotation)
        {
            var q = rotation;
            double ysqr = q.Y * q.Y;

            // pitch (x-axis rotation)
            double t0 = 2.0f * (q.W * q.X + q.Y * q.Z);
            double t1 = 1.0f - 2.0f * (q.X * q.X + ysqr);
            double pitch = Math.Atan2(t0, t1);

            // yaw (y-axis rotation)
            double t2 = 2.0f * (q.W * q.Y - q.Z * q.X);

            t2 = (t2 > 1.0f) ? 1.0f : t2;
            t2 = (t2 < -1.0f) ? -1.0f : t2;

            double yaw = Math.Asin(t2);

            // roll (z-axis rotation)
            double t3 = 2.0f * (q.W * q.Z + q.X * q.Y);
            double t4 = 1.0f - 2.0f * (ysqr + q.Z * q.Z);
            double roll = Math.Atan2(t3, t4);

            return new Vector3((float)pitch, (float)yaw, (float)roll);
        }

        public static Vector3 ToEulerAnglesInDegrees(Quaternion rotation)
        {
            var anglesInRadians = ToEulerAngles(rotation);
            var xInDegrees = MathHelper.RadiansToDegrees(anglesInRadians.X);
            var yInDegrees = MathHelper.RadiansToDegrees(anglesInRadians.Y);
            var zInDegrees = MathHelper.RadiansToDegrees(anglesInRadians.Z);

            return new Vector3(MapTo360(xInDegrees), MapTo360(yInDegrees), MapTo360(zInDegrees));
        }

        // Returns a quaternion such that q * start = dest
        public static Quaternion FromTo(Vector3 from, Vector3 to)
        {
            from.Normalize();
            to.Normalize();

            float cosTheta = Vector3.Dot(from, to);
            Vector3 rotationAxis;

            if (cosTheta > 1 - 0.001f)
            {
                return Quaternion.Identity;
            }
            if (cosTheta < -1 + 0.001f)
            {
                // special case when vectors in opposite directions :
                // there is no "ideal" rotation axis
                // So guess one; any will do as long as it's perpendicular to start
                // This implementation favors a rotation around the Up axis,
                // since it's often what you want to do.
                rotationAxis = Vector3.Cross(new Vector3(0, 0, 1), from);

                if (rotationAxis.LengthSquared < 0.01f) // bad luck, they were parallel, try again!
                {
                    rotationAxis = Vector3.Cross(new Vector3(1, 0, 0), from);
                }

                rotationAxis.Normalize();

                return Quaternion.FromAxisAngle(rotationAxis, MathHelper.DegreesToRadians(180));
            }

            // Implementation from Stan Melax's Game Programming Gems 1 article
            rotationAxis = Vector3.Cross(from, to);

            float s = (float)Math.Sqrt((1 + cosTheta) * 2);
            float invs = 1 / s;

            return new Quaternion(rotationAxis.X * invs, rotationAxis.Y * invs, rotationAxis.Z * invs, s * 0.5f);
        }

        // Returns a quaternion that will make your object looking towards 'direction'.
        // Similar to RotationBetweenVectors, but also controls the vertical orientation.
        // This assumes that at rest, the object faces -Z.
        // Beware, the first parameter is a direction, not the target point !
        public static Quaternion LookAt(Vector3 direction, Vector3 desiredUp)
        {
            if (direction.LengthSquared < 0.0001f)
            {
                return Quaternion.Identity;
            }

            // Recompute desiredUp so that it's perpendicular to the direction
            // You can skip that part if you really want to force desiredUp
            Vector3 right = Vector3.Cross(direction, desiredUp);

            desiredUp = Vector3.Cross(right, direction);

            // Find the rotation between the front of the object (that we assume towards -Z,
            // but this depends on your model) and the desired direction
            Quaternion rot1 = FromTo(-Vector3.UnitZ, direction);

            // Because of the 1rst rotation, the up is probably completely screwed up. 
            // Find the rotation between the "up" of the rotated object, and the desired up
            Vector3 newUp = rot1 * Vector3.UnitY;
            Quaternion rot2 = FromTo(newUp, desiredUp);

            // Apply them
            return rot2 * rot1; // remember, in reverse order.
        }

        // this function can take non-normalized vectors vFrom and vTo (normalizes internally)
        internal static Quaternion FromToOld(Vector3 from, Vector3 to)
        {
            // [TODO] this page seems to have optimized version:
            //    http://lolengine.net/blog/2013/09/18/beautiful-maths-quaternion-from-vectors

            // [RMS] not ideal to explicitly normalize here, but if we don't,
            //   output quaternion is not normalized and this causes problems,
            //   eg like drift if we do repeated SetFromTo()

            from.Normalize();
            to.Normalize();

            var bisector = (from + to).Normalized();
            var ret = Quaternion.Identity;

            ret.W = Vector3.Dot(from, bisector);

            if (ret.W != 0)
            {
                var cross = Vector3.Cross(from, bisector);

                ret.X = cross.X;
                ret.Y = cross.Y;
                ret.Z = cross.Z;
            }
            else
            {
                float invLength;

                if (Math.Abs(from.X) >= Math.Abs(from.Y))
                {
                    // V1.x or V1.z is the largest magnitude component.
                    invLength = (float)(1.0 / Math.Sqrt(from.X * from.X + from.Z * from.Z));
                    ret.X = -from.Z * invLength;
                    ret.Y = 0;
                    ret.Z = from.X * invLength;
                }
                else
                {
                    // V1.y or V1.z is the largest magnitude component.
                    invLength = (float)(1.0 / Math.Sqrt(from.Y * from.Y + from.Z * from.Z));
                    ret.X = 0;
                    ret.Y = from.Z * invLength;
                    ret.Z = -from.Y * invLength;
                }
            }

            return ret.Normalized();
        }

        public static Quaternion FromEulerAngles(Vector3 eulerAngles)
        {
            var rotation = Quaternion.Identity;

            rotation = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(eulerAngles.X)) * rotation;
            rotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(eulerAngles.Y)) * rotation;
            rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(eulerAngles.Z)) * rotation;

            return rotation;
        }

        private static float MapTo360(float degree)
        {
            return (degree < 0) ? degree + 360 : (degree == 360) ? 0 : degree;
        }
    }
}
