using UnityEngine;
using UnityEngine.UIElements;

namespace VisualGeometry.Runtime.Base
{
    public abstract class BaseVisualShape : VisualElement
    {
        public Vector2[] Points;
        public ushort[] Indices;

        protected const float _indent = 0.5f;

        protected Color _color;
        protected Vertex[] _vertices;

        protected BaseVisualShape() => generateVisualContent += GenerateVisualContent;

        ~BaseVisualShape() => generateVisualContent -= GenerateVisualContent;

        public void SetColor(Color color) =>
            _color = color;

        protected virtual void GenerateVisualContent(MeshGenerationContext mgc)
        {
            if (NotValid())
                return;

            SetPosition();
            SetColor();
            SetShape(mgc);
        }

        protected virtual bool NotValid() =>
            Points == null || Indices == null || contentRect.width < 0.1f || contentRect.height < 0.1f;

        protected virtual void SetColor()
        {
            for (var i = 0; i < Points.Length; i++)
                _vertices[i].tint = _color;
        }

        protected virtual void SetPosition()
        {
            _vertices = new Vertex[Points.Length];
            for (var i = 0; i < Points.Length; i++)
                _vertices[i] = new Vertex { position = new Vector3(Points[i].x, Points[i].y) };
        }

        protected virtual void SetShape(MeshGenerationContext mgc)
        {
            var mesh = mgc.Allocate(_vertices.Length, Indices.Length);
            mesh.SetAllVertices(_vertices);
            mesh.SetAllIndices(Indices);
        }
    }
}