using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Shapes
{
    public class Parallelogram : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Parallelogram, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Parallelogram> { }

        public Parallelogram()
        {
            Points = new Vector2[4];
            Indices = new ushort[] { 0, 1, 2, 2, 3, 0 };
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            var indent = rect.width * _indent;

            Points[0] = new Vector3(0, rect.height);
            Points[1] = new Vector3(rect.width - indent, 0f);
            Points[2] = new Vector3(rect.width, 0);
            Points[3] = new Vector3(rect.width - indent, rect.height);

            base.SetPosition();
        }
    }
}