using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Open3D.Render
{
    public struct ColoredVertex
    {
        public static readonly int Size = Marshal.SizeOf(default(ColoredVertex));

        public Vector4 Position { get; }
        public Color4 Color { get; }

        public ColoredVertex(Vector3 position, Color4 color) : this(new Vector4(position, 1), color) { }
        public ColoredVertex(Vector4 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
