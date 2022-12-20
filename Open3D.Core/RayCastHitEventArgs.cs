using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D
{
    public class RayCastHitEventArgs : EventArgs
    {
        public IIntersectsWithRay Value { get; }

        public RayCastHitEventArgs(IIntersectsWithRay value)
        {
            Value = value;
        }
    }
}
