using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class BoxGeometry
    {
        private const int x = 0,y = 1,z = 2;

        public static void getVertices(ref UnityEngine.Mesh mesh, float width, float height, float depth, int segmentsWidth, int segmentsHeight, int segmentsDepth){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            int numberOfVertices = 0;

            buildPlane(ref numberOfVertices, z,y,x,-1,-1,depth,height, width,segmentsDepth,segmentsHeight,ref vertices, ref indices, ref normals,ref uvs);
            buildPlane(ref numberOfVertices, z,y,x, 1,-1,depth,height,-width,segmentsDepth,segmentsHeight,ref vertices, ref indices, ref normals,ref uvs);
            buildPlane(ref numberOfVertices, x,z,y, 1, 1,width,depth, height,segmentsWidth,segmentsDepth,ref vertices, ref indices, ref normals,ref uvs);
            buildPlane(ref numberOfVertices, x,z,y, 1,-1,width,depth,-height,segmentsWidth,segmentsDepth,ref vertices, ref indices, ref normals,ref uvs);
            buildPlane(ref numberOfVertices, x,y,z, 1,-1,width,height, depth,segmentsWidth,segmentsHeight,ref vertices, ref indices, ref normals,ref uvs);
            buildPlane(ref numberOfVertices, x,y,z,-1,-1,width,height,-depth,segmentsWidth,segmentsHeight,ref vertices, ref indices, ref normals,ref uvs);

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = indices.ToArray();
        }

        private static void buildPlane(ref int numberOfVerties, 
                int u, int v, int w,
                int udir,int vdir,float width, float height, float depth, 
                int gridX, int gridY, 
                ref List<Vector3> vertices, ref List<int> indices, 
                ref List<Vector3> normals, ref List<Vector2> uvs)
        {
            float segmentWidth = width / gridX;
            float segmentHeight = height / gridY;

            float widthHalf = width / 2;
            float heightHalf = height / 2;
            float depthHalf = depth / 2;

            int gridX1 = gridX + 1;
            int gridY1 = gridY + 1;

            int vertexCounter = 0;

            int ix,iy;

            Vector3 vector = new Vector3();
            Vector2 vector2 = new Vector2();

            for(iy = 0; iy < gridY1; iy++){
                float y = iy * segmentHeight - heightHalf;
                for(ix = 0; ix < gridX1; ix++){
                    float x = ix * segmentWidth - widthHalf;

                    vector = new Vector3();
                    vector[u] = x * udir;
                    vector[v] = y  * vdir;
                    vector[w] = depthHalf;
                    vertices.Add(vector);

                    vector = new Vector3();
                    vector[u] = 0f;
                    vector[v] = 0f;
                    vector[w] = depth > 0 ? 1 : -1;
                    normals.Add(vector);

                    vector2 = new Vector2();
                    vector2[0] = ix / gridX;
                    vector2[1] = 1 - (iy / gridY);
                    uvs.Add(vector2);

                    vertexCounter += 1;
                }
            }

            for(iy = 0; iy < gridY; iy ++){
                for(ix = 0; ix < gridX; ix ++){
                    int a = numberOfVerties + ix + gridX1 * iy;
                    int b = numberOfVerties + ix + gridX1 * (iy+1);
                    int c = numberOfVerties + (ix+1) + gridX1 * (iy+1);
                    int d = numberOfVerties + (ix+1) + gridX1 * iy;
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(d);
                    indices.Add(b);
                    indices.Add(c);
                    indices.Add(d);
                }
            }
            numberOfVerties += vertexCounter;
        }
    }
}
