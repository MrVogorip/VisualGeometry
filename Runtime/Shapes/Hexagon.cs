using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Shapes
{
    public class Hexagon : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Hexagon, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Hexagon> { }

        public Hexagon()
        {
            Points = new Vector2[6];
            Indices = new ushort[] { 4, 0, 3, 3, 0, 2, 2, 0, 1, 1, 5, 2 };
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            var indentLeft = rect.width * _indent / 2;
            var indentRight = rect.width - indentLeft;
            var halfHeight = rect.height / 2;

            Points[0] = new Vector2(indentLeft, 0);
            Points[1] = new Vector2(indentRight, 0);
            Points[2] = new Vector2(indentRight, rect.height);
            Points[3] = new Vector2(indentLeft, rect.height);
            Points[4] = new Vector2(0, halfHeight);
            Points[5] = new Vector2(rect.width, halfHeight);

            base.SetPosition();
        }
    }
}