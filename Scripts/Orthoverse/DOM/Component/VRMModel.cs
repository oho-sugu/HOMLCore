using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Entity;
using VRM;
using UniGLTF;
using VRMShaders;

namespace Orthoverse.DOM.Component
{
    public class VRMModel : ComponentBase
    {
        private static string _name = "vrm-model";
        private RuntimeGltfInstance instance;

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new VRMModel();
            c.initialize();
            return c;
        }

        public override void Parse(string node){
            ParseUtil.parseAttribute(node, ref attrDic);
        }

        public override void Construct(EntityBase e){
            string objid = attrDic.TryGetValue("obj", out objid) ? objid : "";
            if(e.rootDocument.assetItems.ContainsKey(objid.Replace("#",""))){
                byte[] data = e.rootDocument.assetItems[objid.Replace("#","")];

                Load(e, data);
            }
        }

        private async void Load(EntityBase e, byte[] data)
        {
            this.instance = await VrmUtility.LoadBytesAsync("", data, new RuntimeOnlyAwaitCaller());
            this.instance.transform.SetParent(e.gameObject.transform, false);
            this.instance.ShowMeshes();
        }
    }
}
