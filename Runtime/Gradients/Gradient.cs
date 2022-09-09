using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime.Gradients
{
    public class Gradient : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<Gradient, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<Gradient>
        {
            protected readonly UxmlColorAttributeDescription _right = new() { name = "right-color", defaultValue = Color.black };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as Gradient;

                ate._rightColor = _right.GetValueFromBag(bag, cc);
            }
        }

        protected Color _rightColor;

        public Gradient()
        {
            Points = new Vector2[4];
            Indices = new ushort[] { 0, 1, 2, 2, 3, 0 };
        }

        protected override void SetColor()
        {
            _vertices[0].tint = _color;
            _vertices[1].tint = _color;
            _vertices[2].tint = _rightColor;
            _vertices[3].tint = _rightColor;
        }

        protected override void SetPosition()
        {
            var rect = contentRect;

            Points[0] = new Vector2(0f, rect.height);
            Points[1] = new Vector2(0f, 0f);
            Points[2] = new Vector2(rect.width, 0f);
            Points[3] = new Vector2(rect.width, rect.height);

            base.SetPosition();
        }
    }
}