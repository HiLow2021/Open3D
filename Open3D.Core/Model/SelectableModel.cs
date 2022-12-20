using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Open3D.Render;

namespace Open3D.Model
{
    public class SelectableModel : RenderableModel, IIntersectsWithRay
    {
        protected readonly IRenderer firstRenderer;
        protected readonly IRenderer secondRenderer;

        public SelectableModel(IRenderer firstRenderer, IRenderer secondRenderer, Vector3 position, Vector3 rotation)
            : base(firstRenderer, position, rotation)
        {
            this.firstRenderer = firstRenderer;
            this.secondRenderer = secondRenderer;
        }

        public SelectableModel(IRenderer firstRenderer, IRenderer secondRenderer, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(firstRenderer, position, rotation, scale)
        {
            this.firstRenderer = firstRenderer;
            this.secondRenderer = secondRenderer;
        }

        public void ChangeRenderer()
        {
            Renderer = (Renderer == firstRenderer) ? secondRenderer : firstRenderer;
        }

        public void SetRenderer(bool isFirst)
        {
            Renderer = isFirst ? firstRenderer : secondRenderer;
        }

        public virtual double? IntersectsWithRay(Ray.Ray ray)
        {
            var radius = LocalScale.X;
            var difference = LocalPosition - ray.Origin;
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
    }
}
