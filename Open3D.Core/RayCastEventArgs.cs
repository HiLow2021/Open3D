using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D
{
    public class RayCastEventArgs : EventArgs
    {
        public IIntersectsWithRay Value { get; }

        public RayCastEventArgs(IIntersectsWithRay value)
        {
            Value = value;
        }
    }
}
