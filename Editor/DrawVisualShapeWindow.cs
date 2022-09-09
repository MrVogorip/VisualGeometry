#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VisualGeometry.Editor.Triangulation;
using VisualGeometry.Runtime;

namespace VisualGeometry.Editor
{
    public class DrawVisualShapeWindow : EditorWindow
    {
        private VisualElement _drawingArea;
        private VisualShape _shape;

        private List<Vector2> _points;

        [MenuItem("Window/UI Toolkit/Draw Visual Shape")]
        public static void ShowWindow()
        {
            var window = GetWindow<DrawVisualShapeWindow>();
            window.titleContent = new GUIContent("Draw Visual Shape");
            window.minSize = new Vector2(650, 270);
            window.maxSize = new Vector2(1920, 1080);
        }

        public void CreateGUI()
        {
            _points = new List<Vector2>();

            _drawingArea = new VisualElement();
            _drawingArea.StretchToParentSize();
            _drawingArea.style.top = 40;
            _drawingArea.RegisterCallback<MouseDownEvent>(MouseDrawingCallback);

            _shape = new VisualShape();
            _shape.StretchToParentSize();
            _shape.style.top = 40;

            rootVisualElement.Add(_shape);
            rootVisualElement.Add(new Button(RenderButtonClicked) { text = "Render" });
            rootVisualElement.Add(new Button(ClearButtonClicked) { text = "Clear" });
            rootVisualElement.Add(_drawingArea);
        }

        private void ClearButtonClicked()
        {
            _points.Clear();
            _drawingArea.Clear();
            _shape.SetColor(Color.clear);
            _shape.MarkDirtyRepaint();
        }

        private void MouseDrawingCallback(MouseDownEvent evt)
        {
            _drawingArea.Add(DrawPointLabel(evt.localMousePosition));
            _points.Add(evt.localMousePosition);
        }

        private Label DrawPointLabel(Vector2 point)
        {
            var label = new Label($"({point.x}:{point.y})");
            label.style.position = Position.Absolute;
            label.style.left = point.x;
            label.style.top = point.y;

            return label;
        }

        private void RenderButtonClicked()
        {
            if (_points.Count < 3)
                return;

            var triangles = EarClipping.Triangulate(new List<Vector2>(_points));
            var vertices = new Vertex[_points.Count];
            var indices = new List<ushort>();

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position = new Vector3(_points[i].x, _points[i].y, Vertex.nearZ);
                vertices[i].tint = Color.white;
            }

            for (var i = 0; i < triangles.Count; i++)
            {
                indices.Add(triangles[i].Vertex2.Index);
                indices.Add(triangles[i].Vertex1.Index);
                indices.Add(triangles[i].Vertex3.Index);
            }

            _shape.Indices = indices.ToArray();
            _shape.Points = _points.ToArray();
            _shape.SetColor(Color.white);
            _shape.MarkDirtyRepaint();

            GUIUtility.systemCopyBuffer = JsonUtility.ToJson(_shape);
        }
    }
}
#endif