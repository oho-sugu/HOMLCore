using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class SphereGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, float radius, int segmentsWidth, int segmentsHeight, float phiStart, float phiLength, float thetaStart, float thetaLength){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            segmentsWidth = Mathf.Max(3, segmentsWidth);
            segmentsHeight = Mathf.Max(2,segmentsHeight);

            float thetaEnd = Mathf.Min(thetaStart + thetaLength, Mathf.PI);

            int ix,iy;

            int index = 0;
            List<List<int>> grid = new List<List<int>>();

            var vertex = new Vector3();
            var uv = new Vector2();

            for(iy = 0; iy <= segmentsHeight; iy++){
                List<int> verticesRow = new List<int>();
                float v = (float)iy / (float)segmentsHeight;
                float uOffset = 0f;
                if(iy == 0 && thetaStart == 0f){
                    uOffset = 0.5f / (float)segmentsWidth;
                } else if(iy == segmentsHeight && thetaEnd == Mathf.PI) {
                    uOffset = -0.5f / (float)segmentsWidth;
                }

                for(ix = 0; ix <= segmentsWidth; ix++){
                    float u = (float)ix / (float)segmentsWidth;

                    vertex = new Vector3();
                    vertex.x = radius * Mathf.Cos(Mathf.Deg2Rad*(phiStart + u * phiLength)) * Mathf.Sin(Mathf.Deg2Rad*(thetaStart + v * thetaLength));
                    vertex.y = radius * Mathf.Cos(Mathf.Deg2Rad*(thetaStart + v * thetaLength));
                    vertex.z = radius * Mathf.Sin(Mathf.Deg2Rad*(phiStart + u * phiLength)) * Mathf.Sin(Mathf.Deg2Rad*(thetaStart + v * thetaLength));
                    vertices.Add(vertex);

                    normals.Add(vertex.normalized);
                    uv = new Vector2(u + uOffset, 1-v);
                    uvs.Add(uv);
                    verticesRow.Add(index++);
                }
                grid.Add(verticesRow);
            }

            for(iy = 0; iy < segmentsHeight; iy++){
                for(ix = 0; ix < segmentsWidth; ix++){
                    int a = grid[iy][ix+1];
                    int b = grid[iy][ix];
                    int c = grid[iy+1][ix];
                    int d = grid[iy+1][ix+1];

                    if(iy != 0 || thetaStart > 0){
                        indices.Add(a);
                        indices.Add(b);
                        indices.Add(d);
                    }
                    if(iy != segmentsHeight - 1 || thetaEnd < Mathf.PI){
                        indices.Add(b);
                        indices.Add(c);
                        indices.Add(d);
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = indices.ToArray();
        }
    }
}
