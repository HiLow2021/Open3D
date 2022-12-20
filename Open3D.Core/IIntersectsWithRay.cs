using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Ray;

namespace Open3D
{
    public interface IIntersectsWithRay
    {
        double? IntersectsWithRay(Ray.Ray ray);
    }
}
