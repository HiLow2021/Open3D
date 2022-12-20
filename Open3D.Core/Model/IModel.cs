using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Model
{
    public interface IModel
    {
        int Id { get; }
        ModelBase Root { get; }
        ModelBase Parent { get; set; }
        Vector3 Forward { get; }
        Vector3 Right { get; }
        Vector3 Up { get; }

        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 EulerAngles { get; set; }

        Vector3 LocalPosition { get; set; }
        Quaternion LocalRotation { get; set; }
        Vector3 LocalEulerAngles { get; set; }
        Vector3 LocalScale { get; set; }

        Matrix4 Model { get; }
        Matrix4 LocalModel { get; }

        void DetachChildren();
        void LookAt(Vector3 target);
        void LookAt(Vector3 target, Vector3 desiredUp);
        void Translate(Vector3 translation);
        void Translate(Vector3 translation, Space space);
        void Rotate(Vector3 angles);
        void Rotate(Vector3 angles, Space space);
        void RotateAround(Vector3 axis, float angle);
        void RotateAround(Vector3 target, Vector3 axis, float angle);
    }
}
