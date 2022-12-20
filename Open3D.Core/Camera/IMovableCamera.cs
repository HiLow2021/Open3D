using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Camera
{
    public interface IMovableCamera : ICamera
    {
        void Translate(Vector3 translation);
        void Translate(Vector3 translation, Space space);
        void Rotate(Vector3 angles);
        void Rotate(Vector3 angles, Space space);
        void RotateAround(Vector3 angles);
        void RotateAround(Vector3 angles, Space space);
    }
}
