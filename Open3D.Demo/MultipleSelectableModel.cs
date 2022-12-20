using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D;
using Open3D.Model;
using Open3D.Ray;
using Open3D.Render;

namespace Open3D.Demo
{
    public class MultipleSelectableModel : RenderableModel, IIntersectsWithRay
    {
        private readonly IList<IRenderer> renderers;

        public MultipleSelectableModel(IList<IRenderer> renderers, Vector3 position, Vector3 rotation, Vector3 scale) : base(renderers.Count > 0 ? renderers[0] : null, position, rotation, scale)
        {
            this.renderers = renderers;
        }

        public virtual double? IntersectsWithRay(Ray.Ray ray)
        {
            var radius = LocalScale.X;
            var difference = Position - ray.Origin;
            var differenceLengthSquared = difference.LengthSquared;
            var sphereRadiusSquared = radius * radius;

            if (differenceLengthSquared < sphereRadiusSquared)
            {
                return 0d;
            }

            var distanceAlongRay = Vector3.Dot(ray.Direction, difference);

            if (distanceAlongRay < 0)
            {
                return null;
            }

            var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;
            var result = (dist < 0) ? null : distanceAlongRay - (double?)Math.Sqrt(dist);

            return result;
        }

        public void SetRenderer(int index)
        {
            if (index >= renderers.Count)
            {
                return;
            }

            Renderer = renderers[index];
        }
    }
}
