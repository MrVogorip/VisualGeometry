#if UNITY_EDITOR
namespace VisualGeometry.Editor.Triangulation
{
    public class Triangle
    {
        public TriangleVertex Vertex1 { get; set; }
        public TriangleVertex Vertex2 { get; set; }
        public TriangleVertex Vertex3 { get; set; }
    }
}
#endif