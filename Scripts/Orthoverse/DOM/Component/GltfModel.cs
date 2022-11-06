using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Entity;

using UniGLTF;

namespace Orthoverse.DOM.Component
{
    public class GltfModel : ComponentBase
    {
        private static string _name = "gltf-model";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new GltfModel();
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
                using (var loader = new ImporterContext(new GlbBinaryParser(data, "LOAD_NAME").Parse()))
                {
                    var instance = loader.Load();
                    instance.transform.SetParent(e.gameObject.transform, false);
                    instance.ShowMeshes();
                }
            }
        }
    }
}
