using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Open3D.Camera;
using Open3D.Model;
using Open3D.Ray;
using Open3D.Render;

namespace Open3D.Controls
{
    public partial class Open3DViewer : GLControl
    {
        private readonly bool isDesignMode;
        private bool isLoaded;

        private IRayCalculator rayCalculator;

        private Matrix4 projection = Matrix4.Identity;

        private bool isCameraRotating;
        private bool isCameraMoving;
        private Vector2 mousePrevious;
        private Vector2 mouseCurrent;

        public MovableCamera Camera { get; } = new MovableCamera(new Vector3(0, 0, 10), Vector3.Zero);
        public CameraMode CameraMode { get; set; } = CameraMode.Free;
        public Vector3 CameraRotationPoint { get; set; }
        public Color ClearColor { get; set; } = Color.Black;
        public IList<ModelBase> Models { get; } = new List<ModelBase>();
        public IIntersectsWithRay RayCastModel { get; private set; }

        public event EventHandler<RayCastEventArgs> RayCast;
        public event EventHandler<RayCastHitEventArgs> RayCastHit;
        public event EventHandler<RayCastChangedEventArgs> RayCastChanged;

        public Open3DViewer()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                isDesignMode = true;

                return;
            }

            InitializeComponent();
        }

        ~Open3DViewer()
        {
            foreach (IDisposable model in Models.Where(x => x is IDisposable))
            {
                model.Dispose();
            }
        }

        private void Open3DViewer_Load(object sender, EventArgs e)
        {
            isLoaded = true;
            Initialize();
        }

        private void Open3DViewer_Resize(object sender, EventArgs e)
        {
            if (!isLoaded || isDesignMode)
            {
                return;
            }

            ResetView();
        }

        private void Open3DViewer_Paint(object sender, PaintEventArgs e)
        {
            if (!isLoaded || isDesignMode)
            {
                return;
            }

            MakeCurrent();
            ClearDisplay();
            Render();
            SwapBuffers();
        }

        private void Initialize()
        {
            ResetView();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            rayCalculator = new InvertRayCalculator(Width, Height, Camera, projection);

            MouseDown += (sender, e) =>
            {
                mouseCurrent = new Vector2(e.X, e.Y);

                if (e.Button == MouseButtons.Left)
                {
                    var ray = rayCalculator.CalculateRay(e.X, e.Y);
                    var newRayCastModel = CastRay(ray);

                    RayCast?.Invoke(this, new RayCastEventArgs(newRayCastModel));

                    if (newRayCastModel != null)
                    {
                        RayCastHit?.Invoke(this, new RayCastHitEventArgs(newRayCastModel));
                    }
                    if (newRayCastModel != RayCastModel)
                    {
                        RayCastChanged?.Invoke(this, new RayCastChangedEventArgs(RayCastModel, newRayCastModel));
                        RayCastModel = newRayCastModel;
                    }

                    Refresh();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    isCameraRotating = true;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    isCameraMoving = true;
                }
            };

            MouseUp += (sender, e) =>
            {
                isCameraRotating = false;
                isCameraMoving = false;
                mousePrevious = Vector2.Zero;
            };

            MouseMove += (sender, e) =>
            {
                mousePrevious = mouseCurrent;
                mouseCurrent = new Vector2(e.X, e.Y);

                var delta = mouseCurrent - mousePrevious;

                if (isCameraRotating)
                {
                    delta *= -0.4f;

                    if (CameraMode == CameraMode.Target)
                    {
                        Camera.RotateAround(CameraRotationPoint, new Vector3(delta.Y, delta.X, 0), Space.Self);
                    }
                    else
                    {
                        Camera.Rotate(new Vector3(delta.Y, delta.X, 0));
                    }

                    Refresh();
                }
                else if (isCameraMoving)
                {
                    delta *= 0.1f;
                    Camera.Translate(new Vector3(delta.X, -delta.Y, 0));
                    Refresh();
                }
            };

            MouseWheel += (sender, e) =>
            {
                var delta = e.Delta * 0.01f;

                Camera.Translate(new Vector3(0, 0, -delta));
                Refresh();
            };
        }

        private void ClearDisplay()
        {
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void Render()
        {
            var view = Camera.View;

            foreach (IRenderableModel model in Models.Where(x => x is IRenderableModel))
            {
                GL.UniformMatrix4(model.Renderer.ViewParameterId, false, ref view);
                GL.UniformMatrix4(model.Renderer.ProjectionParameterId, false, ref projection);

                model.Render();
            }
        }

        private void ResetProjection(ProjectionMode mode)
        {
            if (mode == ProjectionMode.Ortho)
            {
                projection = Matrix4.CreateOrthographicOffCenter(-AspectRatio, AspectRatio, -1, 1, 0.1f, 100);
            }
            else
            {
                projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), AspectRatio, 0.1f, 4000);
            }
        }

        private void ResetRay()
        {
            if (rayCalculator != null)
            {
                rayCalculator.DisplayWidth = Width;
                rayCalculator.DisplayHeight = Height;
                rayCalculator.Camera = Camera;
                rayCalculator.Projection = projection;
            }
        }

        private void ResetView()
        {
            ResetView(ClientSize);
        }

        private void ResetView(int width, int height)
        {
            ResetView(new Size(width, height));
        }

        private void ResetView(Size size)
        {
            MakeCurrent();
            GL.Viewport(size);
            ResetProjection(ProjectionMode.FieldOfView);
            ResetRay();
        }

        private IIntersectsWithRay CastRay(Ray.Ray ray)
        {
            IIntersectsWithRay nearestObject = null;
            double? nearestDistance = null;

            foreach (var model in Models.Select(x => x as IIntersectsWithRay))
            {
                var distance = model?.IntersectsWithRay(ray);

                if (!distance.HasValue)
                {
                    continue;
                }
                if (!nearestDistance.HasValue)
                {
                    nearestDistance = distance;
                    nearestObject = model;

                    continue;
                }
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = model;
                }
            }

            return nearestObject;
        }

        public Bitmap GetScreen()
        {
            return GetScreen(Width, Height);
        }

        public Bitmap GetScreen(int width, int height)
        {
            MakeCurrent();

            GL.CreateFramebuffers(1, out int frameBuffer);
            GL.CreateRenderbuffers(1, out int colorRenderBuffer);
            GL.CreateRenderbuffers(1, out int depthRenderBuffer);

            GL.NamedRenderbufferStorage(colorRenderBuffer, RenderbufferStorage.Rgba8, width, height);
            GL.NamedRenderbufferStorage(depthRenderBuffer, RenderbufferStorage.DepthComponent16, width, height);
            GL.NamedFramebufferRenderbuffer(frameBuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, colorRenderBuffer);
            GL.NamedFramebufferRenderbuffer(frameBuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderBuffer);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, frameBuffer);

            ResetView(width, height);
            ClearDisplay();
            Render();

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, frameBuffer);

            var bitmap = GetBitmapFromFrameBuffer(width, height);

            ResetView();

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.DeleteFramebuffer(frameBuffer);
            GL.DeleteRenderbuffer(colorRenderBuffer);
            GL.DeleteRenderbuffer(depthRenderBuffer);

            return bitmap;
        }

        public Bitmap GetScreenOld()
        {
            return GetScreenOld(Width, Height);
        }

        public Bitmap GetScreenOld(int width, int height)
        {
            MakeCurrent();

            GL.GenFramebuffers(1, out int frameBuffer);
            GL.GenRenderbuffers(1, out int colorRenderBuffer);
            GL.GenRenderbuffers(1, out int depthRenderBuffer);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, frameBuffer);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, colorRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Rgba8, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, colorRenderBuffer);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderBuffer);

            ResetView(width, height);
            ClearDisplay();
            Render();

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, frameBuffer);

            var bitmap = GetBitmapFromFrameBuffer(width, height);

            ResetView();

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.DeleteFramebuffer(frameBuffer);
            GL.DeleteRenderbuffer(colorRenderBuffer);
            GL.DeleteRenderbuffer(depthRenderBuffer);

            return bitmap;
        }

        private Bitmap GetBitmapFromFrameBuffer(int width, int height)
        {
            // RGBA なら Format32bppArgb、RGB なら Format24bppRgb を指定してください。
            var pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            var bitmap = new Bitmap(width, height, pixelFormat);
            var flags = System.Drawing.Imaging.ImageLockMode.WriteOnly;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), flags, pixelFormat);

            GL.ReadBuffer(ReadBufferMode.Back);

            // OpenGL では、PixelFormat は Bgra で管理されています。Argb ではないことに注意してください。
            GL.ReadPixels(0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY); // OpenGL では、左下が原点なので Y 座標を反転させます。

            return bitmap;
        }
    }
}
