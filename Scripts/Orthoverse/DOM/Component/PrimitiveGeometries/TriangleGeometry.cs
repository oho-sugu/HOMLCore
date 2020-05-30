using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public static class TriangleGeometry
    {
        public static void getVertices(ref UnityEngine.Mesh mesh, Vector3 a, Vector3 b, Vector3 c){
            var vertices = new Vector3[3];
            var indices = new int[3];
            var normals = new Vector3[3];
            var uvs = new Vector2[3];

            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
            
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            var normal = Vector3.Cross(b - a, c - b).normalized;
            normals[0] = normal;
            normals[1] = normal;
            normals[2] = normal;
            
            var rot = Quaternion.FromToRotation(normal,Vector3.forward);
            var uva = rot*a;
            var uvb = rot*b;
            var uvc = rot*c;

            float minx = Mathf.Min(Mathf.Min(uva.x,uvb.x), uvc.x);
            float miny = Mathf.Min(Mathf.Min(uva.y,uvb.y), uvc.y);
            float maxx = Mathf.Max(Mathf.Max(uva.x,uvb.x), uvc.x);
            float maxy = Mathf.Max(Mathf.Max(uva.y,uvb.y), uvc.y);

            float scalex = maxx - minx;
            float scaley = maxy - miny;
            uvs[0] = new Vector2(
                (uva.x - minx) / scalex,
                (uva.y - miny) / scaley
            );
            uvs[1] = new Vector2(
                (uvb.x - minx) / scalex,
                (uvb.y - miny) / scaley
            );
            uvs[2] = new Vector2(
                (uvc.x - minx) / scalex,
                (uvc.y - miny) / scaley
            );

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = indices;
        }

    }
}
