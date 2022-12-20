using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Open3D.Model
{
    public abstract class ModelBase : IModel
    {
        private static int _NextObjectId = 0;
        private readonly HashSet<ModelBase> children = new HashSet<ModelBase>();
        private ModelBase parent;

        private Matrix4 LocalToWorldMatrix
        {
            get
            {
                var matrix = Matrix4.Identity;
                var parent = Parent;

                while (parent != null)
                {
                    matrix *= parent.LocalModel;
                    parent = parent.Parent;
                }

                return matrix;
            }
        }

        private Matrix4 WorldToLocalMatrix
        {
            get
            {
                var matrix = Matrix4.Identity;
                var parent = Parent;

                while (parent != null)
                {
                    matrix = parent.LocalModel.Inverted() * matrix;
                    parent = parent.Parent;
                }

                return matrix;
            }
        }

        public int Id { get; }
        public ModelBase Root { get; private set; }

        public ModelBase Parent
        {
            get { return parent; }
            set
            {
                if (parent == value)
                {
                    return;
                }

                parent?.children.Remove(this);
                parent = value;
                Root = parent?.Root ?? parent;
                parent?.children.Add(this);
            }
        }

        public Vector3 Forward => Rotation * -Vector3.UnitZ;
        public Vector3 Right => Rotation * Vector3.UnitX;
        public Vector3 Up => Rotation * Vector3.UnitY;

        public Vector3 Position
        {
            get { return Parent == null ? LocalPosition : (new Vector4(LocalPosition, 1) * LocalToWorldMatrix).Xyz; }
            set { LocalPosition = Parent == null ? value : (new Vector4(value, 1) * WorldToLocalMatrix).Xyz; }
        }

        public Quaternion Rotation
        {
            get { return Parent == null ? LocalRotation : Parent.Rotation * LocalRotation; }
            set { LocalRotation = Parent == null ? value : Parent.Rotation.Inverted() * value; }
        }

        public Vector3 EulerAngles
        {
            get { return QuaternionHelper.ToEulerAnglesInDegrees(Rotation); }
            set { Rotation = QuaternionHelper.FromEulerAngles(value); }
        }

        public Vector3 LocalPosition { get; set; }
        public Quaternion LocalRotation { get; set; }
        public Vector3 LocalEulerAngles
        {
            get { return QuaternionHelper.ToEulerAnglesInDegrees(LocalRotation); }
            set { LocalRotation = QuaternionHelper.FromEulerAngles(value); }
        }
        public Vector3 LocalScale { get; set; }
        public Matrix4 Model => LocalModel * LocalToWorldMatrix;

        public Matrix4 LocalModel
        {
            get
            {
                var translation = Matrix4.CreateTranslation(LocalPosition.X, LocalPosition.Y, LocalPosition.Z);
                var rotation = Matrix4.CreateFromQuaternion(LocalRotation);
                var scale = Matrix4.CreateScale(LocalScale);

                return scale * rotation * translation;
            }
        }

        protected ModelBase() : this(Vector3.Zero, Vector3.Zero, new Vector3(1)) { }

        protected ModelBase(Vector3 position) : this(position, Vector3.Zero, new Vector3(1)) { }

        protected ModelBase(Vector3 position, Vector3 rotation) : this(position, rotation, new Vector3(1)) { }

        protected ModelBase(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            LocalPosition = position;
            LocalEulerAngles = rotation;
            LocalScale = scale;
            Id = _NextObjectId++;
        }

        public virtual void DetachChildren()
        {
            foreach (var item in children)
            {
                item.parent = null;
                item.Root = null;
            }

            children.Clear();
        }

        public virtual void LookAt(Vector3 target)
        {
            LookAt(target, Vector3.UnitY);
        }

        public virtual void LookAt(Vector3 target, Vector3 desiredUp)
        {
            LookAt(target, desiredUp, out _);
        }

        protected virtual void LookAt(Vector3 target, Vector3 desiredUp, out bool isSucceeded)
        {
            isSucceeded = false;

            var to = target - Position;

            if (to.LengthSquared < 0.0001f || desiredUp.LengthSquared < 0.0001f)
            {
                return;
            }

            var cosTheta = Vector3.Dot(to.Normalized(), desiredUp.Normalized());

            if (cosTheta > 1 - 0.001f || cosTheta < -1 + 0.001f)
            {
                return;
            }

            var rotation = QuaternionHelper.LookAt(to, desiredUp);

            Rotation = rotation;
            isSucceeded = true;
        }

        public virtual void Translate(Vector3 translation)
        {
            Translate(translation, Space.Self);
        }

        public virtual void Translate(Vector3 translation, Space space)
        {
            if (space == Space.Self)
            {
                var tx = translation.X * Right;
                var ty = translation.Y * Up;
                var tz = -translation.Z * Forward;

                translation = tx + ty + tz;
            }

            LocalPosition += translation;
        }

        public virtual void Rotate(Vector3 angles)
        {
            Rotate(angles, Space.Self);
        }

        public virtual void Rotate(Vector3 angles, Space space)
        {
            var xInRadians = MathHelper.DegreesToRadians(angles.X);
            var yInRadians = MathHelper.DegreesToRadians(angles.Y);
            var zInRadians = MathHelper.DegreesToRadians(angles.Z);
            var anglesInRadians = new Vector3(xInRadians, yInRadians, zInRadians);

            if (space == Space.World)
            {
                LocalRotation = Quaternion.FromEulerAngles(anglesInRadians) * LocalRotation;
            }
            else
            {
                LocalRotation *= Quaternion.FromEulerAngles(anglesInRadians);
            }
        }

        public virtual void RotateAround(Vector3 axis, float angle)
        {
            RotateAround(Vector3.Zero, axis, angle);
        }

        public virtual void RotateAround(Vector3 target, Vector3 axis, float angle)
        {
            var rotation = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angle));

            Translate(-target, Space.World);
            LocalPosition = rotation * LocalPosition;
            LocalRotation = rotation * LocalRotation;
            Translate(target, Space.World);
        }
    }
}
