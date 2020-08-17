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
                    material = new UnityEngine.Material(Shader.Find("Mixed Reality Toolkit/Standard"));
                    material.EnableKeyword("_DIRECTIONAL_LIGHT");
                    material.EnableKeyword("_HOVER_LIGHT");
                    material.EnableKeyword("_SPECULAR_HIGHLIGHTS");

                    // Directional Light
                    material.SetFloat("_DirectionalLight", 1.0f);
                    
                    //color
                    string colorCode = attrDic.TryGetValue("color", out colorCode) ? colorCode : "#FFFFFF";
                    material.SetColor("_Color", ParseUtil.parseColor(colorCode));

                    //texture
                    string texid = attrDic.TryGetValue("src", out texid) ? texid : "";
                    texid = texid.Replace("#","");

                    if(texid !="" && parent.rootDocument.textures.ContainsKey(texid)){
                        material.SetTexture("_MainTex", parent.rootDocument.textures[texid]);
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
