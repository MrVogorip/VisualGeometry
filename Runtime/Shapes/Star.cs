using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Shapes
{
    public class Star : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Star, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Star> { }

        public Star()
        {
            Points = new Vector2[10];
            Indices = new ushort[] { 9, 0, 1, 1, 2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 9, 1, 3, 3, 5, 7, 9, 3, 7 };
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            Points[0] = new Vector2(rect.width * 1.0000f, rect.height * 0.5000f);
            Points[1] = new Vector2(rect.width * 0.6625f, rect.height * 0.6125f);
            Points[2] = new Vector2(rect.width * 0.6625f, rect.height * 1.0000f);
            Points[3] = new Vector2(rect.width * 0.4375f, rect.height * 0.6875f);
            Points[4] = new Vector2(rect.width * 0.0750f, rect.height * 0.8125f);
            Points[5] = new Vector2(rect.width * 0.3000f, rect.height * 0.5000f);
            Points[6] = new Vector2(rect.width * 0.0750f, rect.height * 0.1875f);
            Points[7] = new Vector2(rect.width * 0.4375f, rect.height * 0.3125f);
            Points[8] = new Vector2(rect.width * 0.6625f, rect.height * 0.0000f);
            Points[9] = new Vector2(rect.width * 0.6625f, rect.height * 0.3875f);

            base.SetPosition();
        }
    }
}