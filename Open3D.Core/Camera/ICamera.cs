using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Camera
{
    public interface ICamera
    {
        Vector3 Position { get; }
        Vector3 Target { get; }
        Matrix4 View { get; }
    }
}
