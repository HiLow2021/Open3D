using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D.Controls
{
    public struct CameraSpeed
    {
        public static CameraSpeed Zero => new CameraSpeed();
        public static CameraSpeed One => new CameraSpeed(1);
        public float Forward { get; set; }
        public float Backward { get; set; }
        public float Right { get; set; }
        public float Left { get; set; }
        public float Up { get; set; }
        public float Down { get; set; }
        public float Degree { get; set; }

        public CameraSpeed(float all) : this(all, all, all, all, all, all, all) { }
        public CameraSpeed(float move, float degree) : this(move, move, move, move, move, move, degree) { }

        public CameraSpeed(float forward, float right, float up) : this(forward, forward, right, right, up, up, 1) { }

        public CameraSpeed(float forward, float right, float up, float degree) : this(forward, forward, right, right, up, up, degree) { }

        public CameraSpeed(float forward, float backward, float right, float left, float up, float down, float degree)
        {
            Forward = forward;
            Backward = backward;
            Right = right;
            Left = left;
            Up = up;
            Down = down;
            Degree = degree;
        }
    }
}
