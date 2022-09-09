#if UNITY_EDITOR
using UnityEngine;

namespace VisualGeometry.Editor.Triangulation
{
    public class TriangleVertex
    {
        public Vector2 Position { get; set; }
        public ushort Index { get; set; }
        public TriangleVertex PreviousVertex { get; set; }
        public TriangleVertex FollowingVertex { get; set; }
        public bool IsReflex { get; set; }
        public bool IsConvex { get; set; }
    }
}
#endif