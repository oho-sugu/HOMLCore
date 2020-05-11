using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Visible : ComponentBase
    {
        public bool visible;
        private static string _name = "visible";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Visible();
            c.initialize();
            c.visible = true;
            return c;
        }
        public override void Parse(string value){
            visible = bool.Parse(value);
        }
        public override void Construct(EntityBase e){
            e.gameObject.SetActive(visible);
        }

        public override string Get(string key){
            return visible.ToString();
        }

        public override void Set(string key, string Value){
            visible = bool.Parse(Value);
            parent.gameObject.SetActive(visible);
        }
    }
}
