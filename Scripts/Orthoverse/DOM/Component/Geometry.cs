using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Geometry : ComponentBase
    {
        public UnityEngine.Mesh mesh;
        private static string _name = "geometry";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Geometry();
            c.initialize();
            return c;
        }
        public override void Parse(string value){
            ParseUtil.parseAttribute(value, ref attrDic);
        }

        public override void Construct(EntityBase e){
            mesh = new UnityEngine.Mesh();
            if(attrDic.ContainsKey("buffer")){
                if(!bool.Parse(attrDic["buffer"])){
                    mesh.MarkDynamic();
                }
            }
            
            string primitivename = attrDic.TryGetValue("primitive", out primitivename) ? primitivename : "box";

            switch(primitivename){
                case "box":
                    boxGen(ref mesh);
                    break;
                case "sphere":
                    sphereGen(ref mesh);
                    break;
                case "plane":
                    planeGen(ref mesh);
                    break;
                default:
                    break;
            }

            mesh.RecalculateBounds();

            var meshFilter = e.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }

        private void boxGen(ref UnityEngine.Mesh mesh){
            string value;
            float width = attrDic.TryGetValue("width", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 1f;
            float depth = attrDic.TryGetValue("depth", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 1f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 1f;
            float segmentDepth = attrDic.TryGetValue("segmentdepth", out value) ? float.Parse(value) : 1f;

            BoxGeometry.getVertices(ref mesh, width, height, depth, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight), Mathf.FloorToInt(segmentDepth));
        }

        private void sphereGen(ref UnityEngine.Mesh mesh){
            string value;
            float radius = attrDic.TryGetValue("radius", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 18f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 36f;
            float phiStart = attrDic.TryGetValue("phistart", out value) ? float.Parse(value) : 0f;
            float phiLength = attrDic.TryGetValue("philength", out value) ? float.Parse(value) : 360f;
            float thetaStart = attrDic.TryGetValue("thetastart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetalength", out value) ? float.Parse(value) : 360f;

            SphereGeometry.getVertices(ref mesh, radius, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight),phiStart, phiLength,thetaStart,thetaLength);
        }

        private void planeGen(ref UnityEngine.Mesh mesh){
            string value;
            float width = attrDic.TryGetValue("width", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 1f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 1f;

            PlaneGeometry.getVertices(ref mesh, width, height, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight));
        }
    }
}
