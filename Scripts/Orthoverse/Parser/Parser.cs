using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Runtime.CompilerServices;

using Orthoverse.DOM;
using Orthoverse.DOM.Component;
using Orthoverse.DOM.Entity;


namespace Orthoverse
{
    public class Parser
    {
        private static Parser _instance = new Parser();

        public static Parser getInstance(){
            return _instance;
        }
        private Parser(){
            // Initialize elementsTemplate
            ComponentFactory.init();
            EntityFactory.init();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Document parse(string homl){
            var htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(homl);

            var docGO = new GameObject();
            var doc = docGO.AddComponent<Document>() as Document;
            doc.init(parseRecurse(htmldoc.DocumentNode.ChildNodes, doc),homl);

            return doc;
        }

        private EntityBase[] parseRecurse(HtmlNodeCollection nodes, Document doc){
            List<EntityBase> entities = new List<EntityBase>();
            foreach(HtmlNode node in nodes){
                System.Type entityType;
                if((entityType = EntityTemplate.getEntity(node.Name)) != null){
                    GameObject g = new GameObject();
                    EntityBase entity;
                    entity = g.AddComponent(entityType) as EntityBase;
                    entity.initialize();
                    entity.ParseAttributes(node);
                    entity.Construct();

                    var childs = parseRecurse(node.ChildNodes, doc);
                    foreach(EntityBase child in childs){
                        child.gameObject.transform.SetParent(entity.gameObject.transform,false);
                        entity.addChild(child);
                    }

                    if(node.Id != null && node.Id != ""){
                        doc.addIDList(node.Id, entity);
                    }
                    entities.Add(entity);
                } else {
                    if(node.Name == "#comment" || node.Name == "#text"){
                        //Debug.Log(node.OuterHtml);
                    } if(node.Name == "script"){
                        if(node.GetAttributeValue("type","") == "text/lua"){
                            doc.addScript(node.InnerText);
                        }
                    } else {
                        //Debug.Log(node.Name + " Not found. Do you register EntityFactory?");
                    }
                    entities.AddRange(parseRecurse(node.ChildNodes, doc));
                }
            }
            return entities.ToArray();
        }
    }
}
  