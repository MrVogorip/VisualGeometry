using System;
using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Runtime.Base;

namespace VisualGeometry.Runtime
{
    [Serializable]
    public class VisualShape : BaseVisualShape
    {
        public new class UxmlFactory : UxmlFactory<VisualShape, UxmlTraits> { }

        public new class UxmlTraits : BaseVisualShapeTraits<VisualShape>
        {
            protected readonly UxmlStringAttributeDescription _data = new() { name = "data" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var data = _data.GetValueFromBag(bag, cc);
                if (string.IsNullOrEmpty(data))
                    return;

                var shape = ve as VisualShape;
                var dataJson = JsonUtility.FromJson<VisualShape>(_data.GetValueFromBag(bag, cc));
                shape.Points = dataJson.Points;
                shape.Indices = dataJson.Indices;
            }
        }
    }
}
