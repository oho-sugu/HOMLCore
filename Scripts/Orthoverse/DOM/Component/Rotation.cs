using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Rotation : ComponentBase
    {
        public Vector3 rotation;
        private static string _name = "rotation";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Rotation();
            c.initialize();
            c.rotation = Vector3.zero;
            return c;
        }    

        public override void Parse(string value){
            rotation = ParseUtil.parseVec3(value);
        }
        public override void Construct(EntityBase e){
            e.gameObject.transform.localRotation = Quaternion.Euler(rotation);
        }

        public override string Get(string key){
            switch(key){
                case "x":
                    return rotation.x.ToString();
                case "y":
                    return rotation.y.ToString();
                case "z":
                    return rotation.z.ToString();
                default:
                    return rotation.ToString();
            }
        }

        public override void Set(string key, string value){
            switch(key){
                case "x":
                    rotation.x = float.Parse(value);
                    parent.gameObject.transform.localRotation = Quaternion.Euler(rotation);
                    break;
                case "y":
                    rotation.y = float.Parse(value);
                    parent.gameObject.transform.localRotation = Quaternion.Euler(rotation);
                    break;
                case "z":
                    rotation.z = float.Parse(value);
                    parent.gameObject.transform.localRotation = Quaternion.Euler(rotation);
                    break;
                default:
                    var v3 = ParseUtil.parseVec3(value);
                    rotation.Set(v3.x,v3.y,v3.z);
                    parent.gameObject.transform.localRotation = Quaternion.Euler(rotation);
                    break;
            }
        }
    }
}
