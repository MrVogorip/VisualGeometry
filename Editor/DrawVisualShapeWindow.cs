using System.Collections.Generic;
using System.Linq;
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
        private VisualElement _toolbox;
        private ListView _pointList;
        private int _selectedPointIndex;

        private List<Vector2> _points;
        private string _shapeData;

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
            _drawingArea.RegisterCallback<MouseDownEvent>(MouseDrawingCallback);
            _drawingArea.Insert(0, Shape());

            _toolbox = new VisualElement();
            _toolbox.Add(RenderButton());
            _toolbox.Add(CopyButton());
            _toolbox.Add(ClearButton());
            _toolbox.Add(ListPoints());

            var split = new TwoPaneSplitView(0, 600, TwoPaneSplitViewOrientation.Horizontal);
            split.Add(_drawingArea);
            split.Add(_toolbox);

            rootVisualElement.Add(split);
        }

        private Button RenderButton() => new(RenderButtonClicked) { text = "Render" };

        private Button ClearButton() => new(ClearButtonClicked) { text = "Clear" };

        private Button CopyButton() => new(CopyButtonClicked) { text = "Copy" };

        private VisualShape Shape()
        {
            var shape = new VisualShape();
            shape.style.position = Position.Absolute;
            shape.StretchToParentSize();

            return shape;
        }

        private void CopyButtonClicked() =>
            GUIUtility.systemCopyBuffer = _shapeData;

        private ListView ListPoints()
        {
            _pointList = new ListView
            {
                makeItem = () => ListPointsMake(),
                bindItem = (item, index) => ListPointsBind(item, index),
            };
            _pointList.onSelectionChange += (_) => { _selectedPointIndex = _pointList.selectedIndex; };

            return _pointList;
        }

        private VisualElement ListPointsMake()
        {
            var item = new VisualElement();
            item.style.flexDirection = FlexDirection.Row;

            return item;
        }

        private void ListPointsBind(VisualElement item, int index)
        {
            var x = new TextField();
            var y = new TextField();

            x.style.width = Length.Percent(47);
            y.style.width = Length.Percent(47);

            var point = _points[index];

            x.value = point.x.ToString();
            y.value = point.y.ToString();

            x.RegisterValueChangedCallback(ChangedX);
            y.RegisterValueChangedCallback(ChangedY);

            item.Add(x);
            item.Add(y);
        }

        private void ChangedX(ChangeEvent<string> evt)
        {
            if (!ValidateIndex())
                return;

            var point = _points[_selectedPointIndex];
            point.x = Parse(evt.newValue);
            _points[_selectedPointIndex] = point;

            RepaintLabelPoint(point);
        }
        private void ChangedY(ChangeEvent<string> evt)
        {
            if (!ValidateIndex())
                return;

            var point = _points[_selectedPointIndex];
            point.y = Parse(evt.newValue);
            _points[_selectedPointIndex] = point;

            RepaintLabelPoint(point);
        }

        private void RepaintLabelPoint(Vector3 point)
        {
            var labelPoint = LabelPoint();
            labelPoint.style.left = point.x;
            labelPoint.style.top = point.y;
            labelPoint.text = $"({point.x}:{point.y})";
            labelPoint.MarkDirtyRepaint();
        }

        private Label LabelPoint() =>
            _drawingArea.ElementAt(_selectedPointIndex + 1) as Label;

        private bool ValidateIndex() =>
            _selectedPointIndex >= 0 && _selectedPointIndex < _points.Count;

        private int Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            return int.TryParse(value, out var y) ? y : default;
        }

        private void ClearButtonClicked()
        {
            _points.Clear();
            _drawingArea.Clear();
            _drawingArea.Insert(0, Shape());
            _pointList.Rebuild();
        }

        private void MouseDrawingCallback(MouseDownEvent evt)
        {
            _drawingArea.Add(PaintLabelPoint(evt.localMousePosition));
            _points.Add(evt.localMousePosition);
            _pointList.itemsSource = _points;
            _pointList.Rebuild();
        }

        private Label PaintLabelPoint(Vector2 pos)
        {
            var labelPoint = new Label($"({pos.x}:{pos.y})");
            labelPoint.style.position = Position.Absolute;
            labelPoint.style.left = pos.x;
            labelPoint.style.top = pos.y;

            return labelPoint;
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
                vertices[i].tint = Color.red;
            }

            for (var i = 0; i < triangles.Count; i++)
            {
                indices.Add(triangles[i].Vertex2.Index);
                indices.Add(triangles[i].Vertex1.Index);
                indices.Add(triangles[i].Vertex3.Index);
            }

            var shape = _drawingArea.ElementAt(0) as VisualShape;
            shape.Data = _shapeData = BuildShapeData(indices.ToArray(), vertices);
            shape.Color = Color.red;
            shape.MarkDirtyRepaint();
        }

        private string BuildShapeData(ushort[] indices, Vertex[] vertices)
        {
            var indicesData = string.Join(",", indices);
            var verticesData = string.Join(",", vertices.Select(x => $"{x.position.x};{x.position.y}"));

            return $"indices:{indicesData}|vertices:{verticesData}";
        }
    }
}
