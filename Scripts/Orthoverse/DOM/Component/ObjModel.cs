using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using System.IO;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class ObjModel : ComponentBase
    {
        private static string _name = "obj-model";
        private static char[] sep1 = {' '};
        private static char[] sep2 = {'/'};

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new ObjModel();
            c.initialize();
            return c;
        }

        public override void Parse(string node){
            ParseUtil.parseAttribute(node, ref attrDic);
        }

        public override void Construct(EntityBase e){
            string objid = attrDic.TryGetValue("obj", out objid) ? objid : "";
            string mtlid = attrDic.TryGetValue("mtl", out mtlid) ? mtlid : "";

            Debug.Log(objid);
            if(e.rootDocument.assetItems.ContainsKey(objid.Replace("#",""))){
                byte[] data = e.rootDocument.assetItems[objid.Replace("#","")];
                using(var strreader = new StreamReader(new MemoryStream(data))){
                    var vertices = new List<Vector3>();
                    var indices = new List<int>();
                    var normals = new List<Vector3>();
                    var uvs = new List<Vector2>();

                    int iVert = 0,iIndi = 0,iNorm = 0,iUVs = 0;
                    string line;
                    while((line = strreader.ReadLine()) != null){
                        if(line.StartsWith("#")){
                            // comment
                        } else if(line.StartsWith("v")){
                            var v = ParseUtil.parseVec3(line.Substring(2).Trim());
                            Debug.Log(v);
                            vertices.Add(v);
                            iVert++;
                        } else if(line.StartsWith("vn")){
                            var v = ParseUtil.parseVec3(line.Substring(3).Trim());
                            normals.Add(v);
                            iNorm++;
                        } else if(line.StartsWith("vt")){
                            var v = ParseUtil.parseVec2(line.Substring(3).Trim());
                            uvs.Add(v);
                            iUVs++;
                        } else if(line.StartsWith("f")){
                            string[] str = line.Substring(2).Trim().Split(sep1);
                            indices.Add(int.Parse(str[0].Split(sep2)[0].Trim())-1);
                            indices.Add(int.Parse(str[1].Split(sep2)[0].Trim())-1);
                            indices.Add(int.Parse(str[2].Split(sep2)[0].Trim())-1);
                            iIndi++;
                        }
                    }

                    var mesh = new UnityEngine.Mesh();
                    mesh.vertices = vertices.ToArray();
                    mesh.normals = normals.ToArray();
                    mesh.uv = uvs.ToArray();
                    mesh.triangles = indices.ToArray();
                    mesh.RecalculateBounds();

                    var meshFilter = e.gameObject.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    var meshRenderer = e.gameObject.GetComponent<MeshRenderer>();
                    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                    meshRenderer.receiveShadows = false;
                    meshRenderer.lightProbeUsage = LightProbeUsage.Off;
                    meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
                    meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

                    var bc = e.gameObject.AddComponent<BoxCollider>();
                    bc.center = mesh.bounds.center;
                    bc.size = mesh.bounds.size;
                    bc.isTrigger = true;
                    bc.enabled = false;
                }

            }
        }
    }
}
