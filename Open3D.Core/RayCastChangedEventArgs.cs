using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open3D
{
    public class RayCastChangedEventArgs : EventArgs
    {
        public IIntersectsWithRay NewValue { get; }
        public IIntersectsWithRay OldValue { get; }

        public RayCastChangedEventArgs(IIntersectsWithRay oldValue, IIntersectsWithRay newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
