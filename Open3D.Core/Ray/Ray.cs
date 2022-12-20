using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Ray
{
    public class Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 rayOrigin, Vector3 rayDirection)
        {
            Origin = rayOrigin;
            Direction = rayDirection;
        }
    }
}
