using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Camera;

namespace Open3D.Ray
{
    public interface IRayCalculator
    {
        int DisplayWidth { get; set; }
        int DisplayHeight { get; set; }
        ICamera Camera { get; set; }
        Matrix4 Projection { get; set; }

        Ray CalculateRay(int mouseX, int mouseY);
    }
}
