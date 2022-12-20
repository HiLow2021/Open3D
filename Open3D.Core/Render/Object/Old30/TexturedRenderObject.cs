using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Open3D.Shader;

namespace Open3D.Render.Object.Old30
{
    public class TexturedRenderObject : RenderObjectBase
    {
        private static int MIN_MIPMAPS = 0;
        private static int MIPMAP_LEVEL = 4;
        private int _TextureId;

        public TexturedRenderObject(IReadOnlyCollection<TexturedVertex> vertices, IShaderProgram shaderProgram, string texturePath)
            : base(shaderProgram, vertices.Count)
        {
            using (var texture = (Bitmap)Image.FromFile(texturePath))
            {
                Initialize(vertices, texture);
            }
        }

        public TexturedRenderObject(IReadOnlyCollection<TexturedVertex> vertices, IShaderProgram shaderProgram, Bitmap texture)
            : base(shaderProgram, vertices.Count)
        {
            Initialize(vertices, texture);
        }

        private void Initialize(IReadOnlyCollection<TexturedVertex> vertices, Bitmap texture)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, TexturedVertex.Size * vertices.Count, vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 4 * sizeof(float));

            _TextureId = InitializeTextures(texture);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private static int InitializeTextures(Bitmap texture)
        {
            var width = texture.Width;
            var height = texture.Height;
            var data = LoadTextureFast(texture);

            GL.GenTextures(1, out int tid);
            GL.BindTexture(TextureTarget.Texture2D, tid);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, ref MIN_MIPMAPS);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, ref MIPMAP_LEVEL);

            var textureMinFilter = (int)TextureMinFilter.Linear;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref textureMinFilter);

            var textureMagFilter = (int)TextureMagFilter.Linear;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref textureMagFilter);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return tid;
        }

        private static float[] LoadTexture(Bitmap texture)
        {
            var width = texture.Width;
            var height = texture.Height;
            var ret = new float[width * height * 4];
            var i = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = texture.GetPixel(x, y);

                    ret[i++] = pixel.R / 255f;
                    ret[i++] = pixel.G / 255f;
                    ret[i++] = pixel.B / 255f;
                    ret[i++] = pixel.A / 255f;
                }
            }

            return ret;
        }

        private static float[] LoadTextureFast(Bitmap texture)
        {
            var width = texture.Width;
            var height = texture.Height;
            var flags = System.Drawing.Imaging.ImageLockMode.ReadWrite;
            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            var bitmapData = texture.LockBits(new Rectangle(0, 0, width, height), flags, format);
            var buf = new byte[width * height * 4];

            Marshal.Copy(bitmapData.Scan0, buf, 0, buf.Length);
            texture.UnlockBits(bitmapData);

            return buf.Select(x => x / 255f).ToArray();
        }

        public override void Bind()
        {
            base.Bind();
            GL.BindTexture(TextureTarget.Texture2D, _TextureId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteTexture(_TextureId);
            }

            base.Dispose(disposing);
        }
    }
}
