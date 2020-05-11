using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Scale : ComponentBase
    {
        public Vector3 scale;
        private static string _name = "scale";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Scale();
            c.initialize();
            c.scale = Vector3.one;
            return c;
        }
        
        public override void Parse(string value){
            scale = ParseUtil.parseVec3(value);
        }
        public override void Construct(EntityBase e){
            e.gameObject.transform.localScale = scale;
        }

        public override string Get(string key){
            switch(key){
                case "x":
                    return scale.x.ToString();
                case "y":
                    return scale.y.ToString();
                case "z":
                    return scale.z.ToString();
                default:
                    return scale.ToString();
            }
        }

        public override void Set(string key, string value){
            switch(key){
                case "x":
                    scale.x = float.Parse(value);
                    parent.gameObject.transform.localScale = scale;
                    break;
                case "y":
                    scale.y = float.Parse(value);
                    parent.gameObject.transform.localScale = scale;
                    break;
                case "z":
                    scale.z = float.Parse(value);
                    parent.gameObject.transform.localScale = scale;
                    break;
                default:
                    var v3 = ParseUtil.parseVec3(value);
                    scale.Set(v3.x,v3.y,v3.z);
                    parent.gameObject.transform.localScale = scale;
                    break;
            }
        }
    }
}