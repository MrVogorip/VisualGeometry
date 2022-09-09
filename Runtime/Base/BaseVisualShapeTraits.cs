using UnityEngine;
using UnityEngine.UIElements;

namespace VisualGeometry.Runtime.Base
{
    public class BaseVisualShapeTraits<T> : VisualElement.UxmlTraits where T : BaseVisualShape
    {
        protected readonly UxmlColorAttributeDescription _color = new() { name = "color", defaultValue = Color.black };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var bvs = ve as T;

            bvs.SetColor(_color.GetValueFromBag(bag, cc));
        }
    }
}