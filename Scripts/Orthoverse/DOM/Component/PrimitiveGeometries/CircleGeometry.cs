using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class CircleGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, float radius, int segments, float thetaStart, float thetaLength){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            Vector3 vertex;
            Vector2 uv;

            vertices.Add(Vector3.zero);
            normals.Add(Vector3.forward);
            uvs.Add(new Vector2(0.5f,0.5f));

            for(int s = 0; s <= segments; s++){
                float segment = thetaStart + s * thetaLength / segments;

                vertex = new Vector3(
                    radius * Mathf.Cos(segment * Mathf.Deg2Rad),
                    radius * Mathf.Sin(segment * Mathf.Deg2Rad),
                    0f
                );
                vertices.Add(vertex);
                normals.Add(Vector3.forward);

                uv = new Vector2(
                    (vertex.x / radius + 1f) / 2f,
                    (vertex.y / radius + 1f) / 2f
                );
                uvs.Add(uv);
            }

            for(int i = 1;i <= segments; i++){
                indices.Add(i);
                indices.Add(i+1);
                indices.Add(0);
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = indices.ToArray();
        }

    }
}
