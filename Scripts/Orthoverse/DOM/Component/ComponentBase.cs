using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public abstract class ComponentBase
    {
        protected EntityBase parent;
        protected Dictionary<string,string> attrDic = new Dictionary<string, string>();
        protected string name = "Component";
        public string getName(){
            return name;
        }

        // Parse string and store attrDic or direct parse to local valiable
        public abstract void Parse(string node);
        
        // Construct from attrDic
        public abstract void Construct(EntityBase e);

        public void AddParent(EntityBase e){
            parent = e;
        }
        public virtual void update(){}
        public virtual void start(){}

        public virtual string Get(string key){ return ""; }
        public virtual void Set(string key, string Value){}

        // Initialize exec only once from ComponentFactory
        public virtual void initialize(){}

        // Get New Component
        public virtual ComponentBase newComponent(){return null;}

        public void addAttribute(string name, string value){
            attrDic.Add(name,value);
        }

    }
}
