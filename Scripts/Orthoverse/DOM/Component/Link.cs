﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Link : ComponentBase
    {
        private static string _name = "link";

        private string href,title,target;

        public override void initialize()
        {
            this.name = _name;
        }

        public override ComponentBase newComponent()
        {
            var c = new Link();
            c.initialize();
            return c;
        }

        public override void Parse(string node)
        {
            ParseUtil.parseAttribute(node, ref attrDic);
        }

        public override void Construct(EntityBase e)
        {
            href = attrDic.TryGetValue("href", out href) ? href : "";
            title = attrDic.TryGetValue("title", out title) ? title : "";
            target = attrDic.TryGetValue("target", out target) ? target : "_blank";

            href = href.Trim();
            if(href.StartsWith("urlhttps(")){
                href = "https://"+href.Replace("urlhttps(","").Replace(")","");
            } else if(href.StartsWith("urlhttp(")){
                href = "http://"+href.Replace("urlhttp(","").Replace(")","");
            }

            e.event_click += linkAction;
            e.event_start += linkStart;
        }

        public void linkAction(){
            OpenMode mode;
            switch(target){
                case "_blank":
                    mode = OpenMode.blank;
                    break;
                case "_self":
                    mode = OpenMode.self;
                    break;
                default:
                    mode = OpenMode.blank;
                    break;
            }
            Debug.Log(href);
            this.parent.rootDocument.dm.open(this.parent.rootDocument,new System.Uri(href),mode);
        }

        public void linkStart(EntityBase e){
            var c = e.gameObject.GetComponent<Collider>();
            c.enabled = true;
        }
    }

}