using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Position : ComponentBase
    {
        public Vector3 position;
        private static string _name = "position";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Position();
            c.initialize();
            c.position = Vector3.zero;
            return c;
        }
        public override void Parse(string value){
            position = ParseUtil.parseVec3(value);
        }
        public override void Construct(EntityBase e){
            e.gameObject.transform.localPosition = position;
        }

        public override string Get(string key){
            switch(key){
                case "x":
                    return position.x.ToString();
                case "y":
                    return position.y.ToString();
                case "z":
                    return position.z.ToString();
                default:
                    return position.ToString();
            }
        }

        public override void Set(string key, string value){
            switch(key){
                case "x":
                    position.x = float.Parse(value);
                    parent.gameObject.transform.localPosition = position;
                    break;
                case "y":
                    position.y = float.Parse(value);
                    parent.gameObject.transform.localPosition = position;
                    break;
                case "z":
                    position.z = float.Parse(value);
                    parent.gameObject.transform.localPosition = position;
                    break;
                default:
                    var v3 = ParseUtil.parseVec3(value);
                    position.Set(v3.x,v3.y,v3.z);
                    parent.gameObject.transform.localPosition = position;
                    break;
            }
        }
    }
}
