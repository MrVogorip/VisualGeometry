#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using static VisualGeometry.Editor.Triangulation.TriangleMath;

namespace VisualGeometry.Editor.Triangulation
{
    public static class EarClipping
    {
        public static List<Triangle> Triangulate(List<Vector2> points)
        {
            var triangles = new List<Triangle>();
            if (points.Count == 3)
                return OneTriangle(points);

            var vertices = new List<TriangleVertex>();
            for (var i = 0; i < points.Count; i++)
                vertices.Add(new TriangleVertex { Position = points[i], Index = (ushort)i });

            for (var i = 0; i < vertices.Count; i++)
            {
                vertices[i].PreviousVertex = vertices[Modulo(i - 1, vertices.Count)];
                vertices[i].FollowingVertex = vertices[Modulo(i + 1, vertices.Count)];
            }

            for (var i = 0; i < vertices.Count; i++)
                ConvexOrReflex(vertices[i]);

            var earVertices = new List<TriangleVertex>();
            for (var i = 0; i < vertices.Count; i++)
                IsEarVertex(vertices[i], vertices, earVertices);

            while (vertices.Count >= 3)
            {
                if (vertices.Count == 3)
                {
                    triangles.Add(new Triangle
                    {
                        Vertex1 = vertices[0],
                        Vertex2 = vertices[0].PreviousVertex,
                        Vertex3 = vertices[0].FollowingVertex,
                    });
                    break;
                }

                var earVertex = earVertices[0];
                var earVertexPrevious = earVertex.PreviousVertex;
                var earVertexFollowing = earVertex.FollowingVertex;

                triangles.Add(new Triangle
                {
                    Vertex1 = earVertex,
                    Vertex2 = earVertexPrevious,
                    Vertex3 = earVertexFollowing,
                });

                earVertices.Remove(earVertex);
                vertices.Remove(earVertex);

                earVertexPrevious.FollowingVertex = earVertexFollowing;
                earVertexFollowing.PreviousVertex = earVertexPrevious;

                ConvexOrReflex(earVertexPrevious);
                ConvexOrReflex(earVertexFollowing);

                earVertices.Remove(earVertexPrevious);
                earVertices.Remove(earVertexFollowing);

                IsEarVertex(earVertexPrevious, vertices, earVertices);
                IsEarVertex(earVertexFollowing, vertices, earVertices);
            }

            return triangles;
        }

        private static List<Triangle> OneTriangle(List<Vector2> points)
        {
            return new List<Triangle>()
            {
                new Triangle
                {
                    Vertex1 = new TriangleVertex
                    {
                        Position = points[1],
                        Index = 1,
                    },
                    Vertex2 = new TriangleVertex
                    {
                        Position = points[0],
                        Index = 0,
                    },
                    Vertex3 = new TriangleVertex
                    {
                        Position = points[2],
                        Index = 2,
                    },
                },
            };
        }
    }
}
#endif