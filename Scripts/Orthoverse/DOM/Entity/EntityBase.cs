using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

using MoonSharp.Interpreter;

using Orthoverse.DOM.Entity;
using Orthoverse.DOM.Component;

namespace Orthoverse.DOM.Entity
{
    public delegate void OnClick();
    public delegate void OnOver();
    public delegate void OnOut();
    public delegate void OnStart(EntityBase e);

    [MoonSharpUserData]
    public class EntityBase : MonoBehaviour
    {
        protected Dictionary<string,ComponentAttribute> attrAlias;
        protected Dictionary<string,ComponentBase> components = new Dictionary<string, ComponentBase>();
        protected List<EntityBase> children = new List<EntityBase>();

        public Document rootDocument;

        public OnClick event_click;
        public OnOver event_over;
        public OnOut event_out;
        public OnStart event_start;

        public void ParseAttributes(HtmlNode node){
            var attrs = node.GetAttributes();
            foreach(var attr in attrs){
                if(attrAlias.ContainsKey(attr.Name)){
                    if(components.ContainsKey(attrAlias[attr.Name].component)){
                        components[attrAlias[attr.Name].component].addAttribute(attrAlias[attr.Name].property, attr.Value);
                    } else {
                        var c = ComponentTemplate.getComponent(attrAlias[attr.Name].component);
                        if(c != null){
                            c.addAttribute(attrAlias[attr.Name].property, attr.Value);
                            components.Add(c.getName(), c);
                        }
                    }
                } else {
                    if(components.ContainsKey(attr.Name)){
                        components[attr.Name].Parse(attr.Value);
                    } else {
                        var c = ComponentTemplate.getComponent(attr.Name);
                        if(c != null){
                            c.Parse(attr.Value);
                            components.Add(c.getName(), c);
                        } else {
                            //Debug.Log(attr.Name + " Not found. Do you register Component Factory?");
                        }
                    }
                }
            }
        }

        public ComponentBase GetComponentBase(string name){
            ComponentBase v;
            v = components.TryGetValue(name, out v) ? v : null;
            return v;
        }

        protected void addComponentMapping(string componentName, string property){
            var attrName = Regex.Replace(property, "([a-z])([A-Z])", "$1-$2");
            if(attrAlias.ContainsKey(attrName)){
                attrName = componentName + "-" + property;
            }
            attrAlias.Add(attrName, new ComponentAttribute(componentName,property));
        }

        // Initialize exec only once from EntityFactory
        public virtual void initialize(){
            return;
        }

        // Construct Entity
        // - Construct Unity Game Object
        // - Construct Component and link to GameObject
        public void Construct(Document d){
            this.rootDocument = d;
            foreach(var c in components.Values){
                c.AddParent(this);
                c.Construct(this);
            }
            this.Construct();
            return;
        }

        public virtual void Construct(){
            return;
        }

        

        void Start(){
            event_start?.Invoke(this);
            event_start = null;

            foreach(ComponentBase c in components.Values){
                c.start();
            }
        }
        public void addChild(EntityBase child){
            children.Add(child);
        }

        public void OnClickHandler(){
            event_click?.Invoke();
        }
        public void OnOverHandler(){
            event_over?.Invoke();
        }
        public void OnOutHandler(){
            event_out?.Invoke();
        }
    }

    public struct ComponentAttribute{
        public string component;
        public string property;
        public ComponentAttribute(string _component, string _property){
            component = _component;
            property = _property;
        }

        public override string ToString(){
            return component + " : " + property;
        }
    }
}
