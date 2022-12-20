using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Open3D.Render
{
    public struct TexturedVertex
    {
        public static readonly int Size = Marshal.SizeOf(default(TexturedVertex));

        public Vector4 Position { get; }
        public Vector2 TextureCoordinate { get; }

        public TexturedVertex(Vector3 position, Vector2 textureCoordinate) : this(new Vector4(position, 1), textureCoordinate) { }
        public TexturedVertex(Vector4 position, Vector2 textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }
    }
}
