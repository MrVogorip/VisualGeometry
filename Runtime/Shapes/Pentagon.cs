using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Shapes
{
    public class Pentagon : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Pentagon, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Pentagon> { }

        public Pentagon()
        {
            Points = new Vector2[5];
            Indices = new ushort[] { 0, 4, 1, 1, 2, 3, 2, 1, 4 };
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            Points[0] = new Vector2(rect.width / 2, 0f);
            Points[1] = new Vector2(0, rect.height * _indent);
            Points[4] = new Vector2(rect.width, rect.height * _indent);
            Points[2] = new Vector2(rect.width * 0.66f, rect.height);
            Points[3] = new Vector2(rect.width * 0.33f, rect.height);

            base.SetPosition();
        }
    }
}