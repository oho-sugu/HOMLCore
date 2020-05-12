using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class TorusGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, float radius, float tube, int segmentsRadial, int segmentsTubular, float arc){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            Vector3 center,vertex,normal;
            int j,i;

            for(j=0;j<=segmentsRadial;j++){
                for(i=0;i<=segmentsTubular;i++){
                    float u = (float)i / (float)segmentsTubular * arc;
                    float v = (float)j / (float)segmentsRadial * Mathf.PI * 2;

                    vertex = new Vector3(
                        (radius + tube*Mathf.Cos(v))*Mathf.Cos(u*Mathf.Deg2Rad),
                        (radius + tube*Mathf.Cos(v))*Mathf.Sin(u*Mathf.Deg2Rad),
                        tube * Mathf.Sin(v)
                    );
                    vertices.Add(vertex);

                    center = new Vector3(
                        radius * Mathf.Cos(u*Mathf.Deg2Rad),
                        radius * Mathf.Sin(u*Mathf.Deg2Rad),
                        0f
                    );
                    normal = (vertex - center).normalized;
                    normals.Add(normal);

                    uvs.Add(new Vector2((float)i / (float)segmentsTubular,(float)j / (float)segmentsRadial));

                }
            }

            for(j=1;j<=segmentsRadial;j++){
                for(i=1;i<=segmentsTubular;i++){
                    int a = (segmentsTubular+1)*j+i-1;
                    int b = (segmentsTubular+1)*(j-1)+i-1;
                    int c = (segmentsTubular+1)*(j-1)+i;
                    int d = (segmentsTubular+1)*j+i;

                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(d);
                    indices.Add(b);
                    indices.Add(c);
                    indices.Add(d);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = indices.ToArray();
        }
    }
}
