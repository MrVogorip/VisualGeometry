using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualGeometry.Runtime
{
    public class VisualShape : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VisualShape, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _data = new() { name = "data" };
            private readonly UxmlColorAttributeDescription _color = new() { name = "color" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var shape = ve as VisualShape;

                shape.Data = _data.GetValueFromBag(bag, cc);
                shape.Color = _color.GetValueFromBag(bag, cc);
            }
        }

        private Vertex[] _vertices;
        private ushort[] _indices;

        public Color Color { get; set; }

        public string Data
        {
            get => ComposeData();
            set => ParseData(value);
        }

        public VisualShape() => generateVisualContent += GenerateVisualContent;

        ~VisualShape() => generateVisualContent -= GenerateVisualContent;

        private void GenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_vertices == null || _indices == null)
                return;

            var rect = contentRect;
            if (rect.width < 0.1f || rect.height < 0.1f)
                return;

            for (var i = 0; i < _vertices.Length; i++)
                _vertices[i].tint = Color;

            var mesh = mgc.Allocate(_vertices.Length, _indices.Length);
            mesh.SetAllVertices(_vertices);
            mesh.SetAllIndices(_indices);
        }

        private void ParseData(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return;

                var dataSplit = data.Split("|");
                var indicesData = dataSplit[0].Replace("indices:", string.Empty).Split(",");
                var verticesData = dataSplit[1].Replace("vertices:", string.Empty).Split(",");
                var indices = indicesData.Select(x => ushort.Parse(x)).ToArray();
                var vertices = verticesData.Select(vertex =>
                {
                    var point = vertex.Split(";");
                    var x = int.Parse(point[0]);
                    var y = int.Parse(point[1]);
                    return new Vertex { position = new Vector3(x, y, Vertex.nearZ) };
                }).ToArray();

                _indices = indices;
                _vertices = vertices;
            }
            catch (Exception e)
            {
                Debug.LogError($"Parse Data Exception Message: {e.Message}");
            }
        }

        private string ComposeData()
        {
            if (_vertices == null || _indices == null)
                return string.Empty;

            var indicesData = string.Join(",", _indices);
            var verticesData = string.Join(",", _vertices.Select(x => $"{x.position.x};{x.position.y}"));
            return $"indices:{indicesData}|vertices:{verticesData}";
        }
    }
}
