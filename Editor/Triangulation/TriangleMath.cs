#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace VisualGeometry.Editor.Triangulation
{
    public static class TriangleMath
    {
        public static int Modulo(int index, int verticesCount) =>
            ((index % verticesCount) + verticesCount) % verticesCount;

        public static void ConvexOrReflex(TriangleVertex vertex)
        {
            var previous = vertex.PreviousVertex.Position;
            var current = vertex.Position;
            var following = vertex.FollowingVertex.Position;

            vertex.IsReflex = false;
            vertex.IsConvex = false;

            if (OrientedClockwise(previous, current, following))
                vertex.IsReflex = true;
            else
                vertex.IsConvex = true;
        }

        public static void IsEarVertex(TriangleVertex vertex, List<TriangleVertex> vertices, List<TriangleVertex> earVertices)
        {
            if (vertex.IsReflex)
                return;

            var innerPoint = false;
            for (var i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].IsReflex && PointInTriangle(vertex, vertices[i].Position))
                {
                    innerPoint = true;
                    break;
                }
            }

            if (!innerPoint)
                earVertices.Add(vertex);
        }

        private static bool PointInTriangle(TriangleVertex vertex, Vector2 point)
        {
            var v1 = vertex.PreviousVertex.Position;
            var v2 = vertex.Position;
            var v3 = vertex.FollowingVertex.Position;

            var div = (v2.y - v3.y) * (v1.x - v3.x) + (v3.x - v2.x) * (v1.y - v3.y);

            var d1 = ((v2.y - v3.y) * (point.x - v3.x) + (v3.x - v2.x) * (point.y - v3.y)) / div;
            var d2 = ((v3.y - v1.y) * (point.x - v3.x) + (v1.x - v3.x) * (point.y - v3.y)) / div;
            var d3 = 1 - d1 - d2;

            return d1 > 0f && d1 < 1f && d2 > 0f && d2 < 1f && d3 > 0f && d3 < 1f;
        }

        private static bool OrientedClockwise(Vector2 p, Vector2 c, Vector2 f) =>
            p.x * c.y + f.x * p.y + c.x * f.y - p.x * f.y - f.x * c.y - c.x * p.y <= 0f;
    }
}
#endif