using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
                    boxGen(ref mesh, e.gameObject);
                    break;
                case "sphere":
                    sphereGen(ref mesh, e.gameObject);
                    break;
                case "plane":
                    planeGen(ref mesh, e.gameObject);
                    break;
                case "cylinder":
                    cylinderGen(ref mesh, e.gameObject);
                    break;
                case "cone":
                    coneGen(ref mesh, e.gameObject);
                    break;
                case "torus":
                    torusGen(ref mesh, e.gameObject);
                    break;
                case "circle":
                    circleGen(ref mesh, e.gameObject);
                    break;
                case "ring":
                    ringGen(ref mesh, e.gameObject);
                    break;
                case "triangle":
                    triangleGen(ref mesh, e.gameObject);
                    break;
                default:
                    break;
            }

            var meshFilter = e.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = e.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        }

        private void boxGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float width = attrDic.TryGetValue("width", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 1f;
            float depth = attrDic.TryGetValue("depth", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 1f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 1f;
            float segmentDepth = attrDic.TryGetValue("segmentdepth", out value) ? float.Parse(value) : 1f;

            BoxGeometry.getVertices(ref mesh, width, height, depth, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight), Mathf.FloorToInt(segmentDepth));

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            bc.center = Vector3.zero;
            bc.size = new Vector3(width,height,depth);
            bc.isTrigger = true;
            bc.enabled = false;
        }

        private void sphereGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float radius = attrDic.TryGetValue("radius", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 18f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 36f;
            float phiStart = attrDic.TryGetValue("phistart", out value) ? float.Parse(value) : 0f;
            float phiLength = attrDic.TryGetValue("philength", out value) ? float.Parse(value) : 360f;
            float thetaStart = attrDic.TryGetValue("thetastart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetalength", out value) ? float.Parse(value) : 360f;

            SphereGeometry.getVertices(ref mesh, radius, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight),phiStart, phiLength,thetaStart,thetaLength);

            mesh.RecalculateBounds();

            if(phiStart!=0f || phiLength!=360f || thetaStart!=0f || thetaLength!=360f ){
                var bc = go.AddComponent<BoxCollider>();
                var bound = mesh.bounds;
                bc.center = bound.center;
                bc.size = bound.size;
                bc.isTrigger = true;
                bc.enabled = false;
            } else {
                var sc = go.AddComponent<SphereCollider>();
                sc.center = Vector3.zero;
                sc.radius = radius;
                sc.isTrigger = true;
                sc.enabled = false;
            }
        }

        private void planeGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float width = attrDic.TryGetValue("width", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 1f;
            float segmentWidth = attrDic.TryGetValue("segmentwidth", out value) ? float.Parse(value) : 1f;
            float segmentHeight = attrDic.TryGetValue("segmentheight", out value) ? float.Parse(value) : 1f;

            PlaneGeometry.getVertices(ref mesh, width, height, Mathf.FloorToInt(segmentWidth), Mathf.FloorToInt(segmentHeight));

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            bc.center = Vector3.zero;
            bc.size = new Vector3(width,height,0.05f);
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void cylinderGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float radius = attrDic.TryGetValue("radius", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 2f;
            float segmentsRadial = attrDic.TryGetValue("segmentsRadial", out value) ? float.Parse(value) : 36f;
            float segmentsHeight = attrDic.TryGetValue("segmentsHeight", out value) ? float.Parse(value) : 18f;
            bool openEnded = attrDic.TryGetValue("openEnded", out value) ? bool.Parse(value) : false;   
            float thetaStart = attrDic.TryGetValue("thetaStart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetaLength", out value) ? float.Parse(value) : 360f;

            CylinderGeometry.getVertices(ref mesh, height, openEnded, radius, radius, Mathf.FloorToInt(segmentsRadial), Mathf.FloorToInt(segmentsHeight),thetaStart,thetaLength);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            bc.size = bound.size;
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void coneGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float radiusTop = attrDic.TryGetValue("radiusTop", out value) ? float.Parse(value) : 1f;
            float radiusBottom = attrDic.TryGetValue("radiusBottom", out value) ? float.Parse(value) : 1f;
            float height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 2f;
            float segmentsRadial = attrDic.TryGetValue("segmentsRadial", out value) ? float.Parse(value) : 36f;
            float segmentsHeight = attrDic.TryGetValue("segmentsHeight", out value) ? float.Parse(value) : 18f;
            bool openEnded = attrDic.TryGetValue("openEnded", out value) ? bool.Parse(value) : false;   
            float thetaStart = attrDic.TryGetValue("thetaStart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetaLength", out value) ? float.Parse(value) : 360f;

            CylinderGeometry.getVertices(ref mesh, height, openEnded, radiusBottom, radiusTop, Mathf.FloorToInt(segmentsRadial), Mathf.FloorToInt(segmentsHeight),thetaStart,thetaLength);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            bc.size = bound.size;
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void torusGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float radius = attrDic.TryGetValue("radius", out value) ? float.Parse(value) : 1f;
            float radiusTubular = attrDic.TryGetValue("radiusTubular", out value) ? float.Parse(value) : 0.2f;
            float segmentsRadial = attrDic.TryGetValue("segmentsRadial", out value) ? float.Parse(value) : 36f;
            float segmentsTubular = attrDic.TryGetValue("segmentsTubular", out value) ? float.Parse(value) : 32f;
            float arc = attrDic.TryGetValue("arc", out value) ? float.Parse(value) : 360f;

            TorusGeometry.getVertices(ref mesh, radius,radiusTubular,Mathf.FloorToInt(segmentsRadial), Mathf.FloorToInt(segmentsTubular),arc);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            bc.size = bound.size;
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void circleGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float radius = attrDic.TryGetValue("radius", out value) ? float.Parse(value) : 1f;
            int segments = attrDic.TryGetValue("segments", out value) ? Mathf.Max(3,int.Parse(value)) : 32;
            float thetaStart = attrDic.TryGetValue("thetaStart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetaLength", out value) ? float.Parse(value) : 360f;

            CircleGeometry.getVertices(ref mesh, radius,segments, thetaStart, thetaLength);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            bc.size = new Vector3(bound.size.x, bound.size.y, 0.05f);
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void ringGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            float innerRadius = attrDic.TryGetValue("radiusInner", out value) ? float.Parse(value) : 1f;
            float outerRadius = attrDic.TryGetValue("radiusOuter", out value) ? float.Parse(value) : 1f;
            int segmentsTheta = attrDic.TryGetValue("segmentsTheta", out value) ? Mathf.Max(3,int.Parse(value)) : 32;
            int segmentsPhi = attrDic.TryGetValue("segmentsPhi", out value) ? Mathf.Max(1,int.Parse(value)) : 8;
            float thetaStart = attrDic.TryGetValue("thetaStart", out value) ? float.Parse(value) : 0f;
            float thetaLength = attrDic.TryGetValue("thetaLength", out value) ? float.Parse(value) : 360f;

            RingGeometry.getVertices(ref mesh, innerRadius, outerRadius, segmentsTheta, segmentsPhi, thetaStart, thetaLength);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            bc.size = new Vector3(bound.size.x, bound.size.y, 0.05f);
            bc.isTrigger = true;
            bc.enabled = false;
        }
        private void triangleGen(ref UnityEngine.Mesh mesh, GameObject go){
            string value;
            Vector3 a = attrDic.TryGetValue("vertexA", out value) ? ParseUtil.parseVec3(value) : new Vector3(0f,0.5f,0f);
            Vector3 b = attrDic.TryGetValue("vertexB", out value) ? ParseUtil.parseVec3(value) : new Vector3(-0.5f,-0.5f,0f);
            Vector3 c = attrDic.TryGetValue("vertexC", out value) ? ParseUtil.parseVec3(value) : new Vector3(0.5f,-0.5f,0f);

            TriangleGeometry.getVertices(ref mesh, a, b, c);

            mesh.RecalculateBounds();

            var bc = go.AddComponent<BoxCollider>();
            var bound = mesh.bounds;
            bc.center = bound.center;
            if(Mathf.Abs(bound.size.z) < 0.01f){
                bc.size = new Vector3(bound.size.x, bound.size.y, 0.05f);
            } else {
                bc.size = bound.size;
            }
            bc.isTrigger = true;
            bc.enabled = false;
        }
    }
}
