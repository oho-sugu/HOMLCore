using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class PlaneGeometry
    {
        private const int x = 0,y = 1,z = 2;

        public static void getVertices(ref UnityEngine.Mesh mesh, float width, float height, int segmentsWidth, int segmentsHeight){
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            var widthHalf = width / 2f;
            var heightHalf = height / 2f;

            var gridX = segmentsWidth;
            var gridY = segmentsHeight;

            var gridX1 = gridX + 1;
            var gridY1 = gridY + 1;

            var segment_width = width / gridX;
            var segment_height = height / gridY;

            int ix,iy;

            for(iy = 0;iy < gridY1; iy++){
                var y = iy * segment_height - heightHalf;
                for(ix = 0;ix < gridX1; ix++){
                    var x = ix * segment_width - widthHalf;
                    vertices.Add(new Vector3(x,-y,0f));
                    normals.Add(new Vector3(0f,0f,1f));
                    uvs.Add(new Vector2((float)ix/(float)gridX, 1f - ((float)iy/(float)gridY)));
                }
            }

            for(iy = 0;iy < gridY; iy++){
                for(ix = 0;ix < gridX; ix++){
                    int a = ix + gridX1 * iy;
                    int b = ix + gridX1 * (iy + 1);
                    int c = (ix+1) + gridX1 * (iy+1);
                    int d = (ix+1) + gridX1 * iy;

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
