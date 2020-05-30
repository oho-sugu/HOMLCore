using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class RingGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, float innerRadius, float outerRadius, int segmentsTheta, int segmentsPhi, float thetaStart, float thetaLength){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            Vector3 vertex;
            Vector2 uv;

            float radius = innerRadius;
            float radiusStep = ((outerRadius - innerRadius)/ segmentsPhi);

            for(int j = 0;j <= segmentsPhi; j++){
                for(int i = 0;i <= segmentsTheta; i++){
                    float segment = thetaStart + i * thetaLength / segmentsTheta;

                    vertex = new Vector3(
                        vertex.x = radius * Mathf.Cos(segment * Mathf.Deg2Rad),
                        vertex.y = radius * Mathf.Sin(segment * Mathf.Deg2Rad),
                        0f
                    );
                    vertices.Add(vertex);
                    normals.Add(Vector3.forward);

                    uv = new Vector2(
                        (vertex.x / outerRadius + 1f) / 2f,
                        (vertex.y / outerRadius + 1f) / 2f
                    );
                    uvs.Add(uv);
                }
                radius += radiusStep;
            }

            for(int j = 0; j < segmentsPhi; j++){
                int thetaSegmentLevel = j * (segmentsTheta + 1);
                for(int i = 0;i < segmentsTheta; i++){
                    int segment = i + thetaSegmentLevel;

                    indices.Add(segment);
                    indices.Add(segment + segmentsTheta + 1);
                    indices.Add(segment + 1);
                    indices.Add(segment + segmentsTheta + 1);
                    indices.Add(segment + segmentsTheta + 2);
                    indices.Add(segment + 1);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = indices.ToArray();
        }

    }
}
