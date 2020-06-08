using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Material : ComponentBase
    {
        public UnityEngine.Material material;
        private static string _name = "material";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Material();
            c.initialize();
            return c;
        }
        public override void Parse(string value){
            ParseUtil.parseAttribute(value, ref attrDic);
        }

        public override void Construct(EntityBase e){
            string shadername = attrDic.TryGetValue("shader", out shadername) ? shadername : "standard";

            switch(shadername){
                case "flat":
                case "standard":
                    material = new UnityEngine.Material(Shader.Find("Legacy Shaders/VertexLit"));
                    
                    //color
                    string colorCode = attrDic.TryGetValue("color", out colorCode) ? colorCode : "#FFFFFF";
                    material.color = ParseUtil.parseColor(colorCode);

                    //texture
                    string texid = attrDic.TryGetValue("src", out texid) ? texid : "";
                    texid = texid.Replace("#","");

                    if(texid !="" && parent.rootDocument.textures.ContainsKey(texid)){
                        material.mainTexture = parent.rootDocument.textures[texid];
                    }

                    break;
                default:
                    break;
            }
            var renderer = e.gameObject.AddComponent<MeshRenderer>();
            renderer.material = material;
        }
    }
}
