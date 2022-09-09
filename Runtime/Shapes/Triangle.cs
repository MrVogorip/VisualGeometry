using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Shapes
{
    public class Triangle : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Triangle, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Triangle> { }

        public Triangle()
        {
            Points = new Vector2[3];
            Indices = new ushort[] { 0, 1, 2 };
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            var indent = rect.width * _indent / 2;

            Points[0] = new Vector2(rect.width / 2, 0f);
            Points[1] = new Vector2(rect.width - indent, rect.height);
            Points[2] = new Vector2(indent, rect.height);

            base.SetPosition();
        }
    }
}