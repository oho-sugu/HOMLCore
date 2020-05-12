using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public class CylinderGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, float height, bool openEnded, float radiusBottom, float radiusTop, int segmentsRadial, int segmentsHeight, float thetaStart, float thetaLength){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            int index = 0;
            var indexArray = new List<List<int>>();
            float halfHeight = height / 2f;

            int x,y;
            Vector3 normal;
            Vector3 vertex;
            float slope = (radiusBottom - radiusTop) / height;

            for(y = 0; y <= segmentsHeight; y++){
                var indexRow = new List<int>();
                float v = (float)y / (float)segmentsHeight;
                float radius = v * (radiusBottom - radiusTop) + radiusTop;

                for(x = 0; x <= segmentsRadial; x++){
                    float u = (float)x / (float)segmentsRadial;
                    float theta = u * thetaLength + thetaStart;



                    float sinTheta = Mathf.Sin(theta*Mathf.Deg2Rad);
                    float cosTheta = Mathf.Cos(theta*Mathf.Deg2Rad);

                    vertex = new Vector3(
                        radius * sinTheta,
                        -v * height + halfHeight,
                        radius * cosTheta
                    );
                    vertices.Add(vertex);

                    normal = new Vector3(sinTheta, slope,cosTheta).normalized;
                    normals.Add(normal);

                    uvs.Add(new Vector2(u, 1-v));

                    indexRow.Add(index++);
                }
                indexArray.Add(indexRow);
            }

            for(x = 0; x < segmentsRadial; x++){
                for(y = 0; y < segmentsHeight; y++){
                    int a = indexArray[y][x];
                    int b = indexArray[y+1][x];
                    int c = indexArray[y+1][x+1];
                    int d = indexArray[y][x+1];

                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(d);
                    indices.Add(b);
                    indices.Add(c);
                    indices.Add(d);
                }
            }

            if(!openEnded){
                for(int ii=0;ii < 2;ii++){
                    bool top = ii==0?true:false;

                    int centerIndexStart,centerIndexEnd;

                    float radius = (top)? radiusTop:radiusBottom;
                    float sign = (top)? 1f:-1f;

                    centerIndexStart = index;
                    for(x = 1; x<=segmentsRadial;x++){
                        vertices.Add(new Vector3(0,halfHeight*sign,0));
                        normals.Add(new Vector3(0,sign,0));
                        uvs.Add(new Vector2(0.5f,0.5f));
                        index++;
                    }
                    centerIndexEnd = index;

                    for(x = 0;x <= segmentsRadial;x++){
                        float u = (float)x / (float)segmentsRadial;
                        float theta = u * thetaLength + thetaStart;

                        float sinTheta = Mathf.Sin(theta*Mathf.Deg2Rad);
                        float cosTheta = Mathf.Cos(theta*Mathf.Deg2Rad);

                        vertices.Add(new Vector3(
                            radius*sinTheta,
                            halfHeight*sign,
                            radius*cosTheta
                        ));
                        normals.Add(new Vector3(0,sign,0));
                        uvs.Add(new Vector2((cosTheta*0.5f)+0.5f,(sinTheta*0.5f)+0.5f));

                        index++;
                    }

                    for(x = 0;x<segmentsRadial;x++){
                        int c = centerIndexStart + x;
                        int i = centerIndexEnd+x;
                        if(top){
                            indices.Add(i);
                            indices.Add(i+1);
                            indices.Add(c);
                        } else{
                            indices.Add(i+1);
                            indices.Add(i);
                            indices.Add(c);
                        }
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
